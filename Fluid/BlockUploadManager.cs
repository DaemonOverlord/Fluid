using Fluid.Room;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    /// <summary>
    /// The asynchronous block upload manager
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Uploading = {Uploading}, Queued = {BlocksQueued}")]
    public class BlockUploadManager
    {
        private List<BlockRequest> m_Queue;
        private WorldConnection m_WorldConnection;
        private Thread m_uploadThread;
        private ManualResetEvent m_ResetEvent;
        private ManualResetEvent m_TimeoutEvent;

        /// <summary>
        /// The event when blocks are finished being uploaded
        /// </summary>
        public event EventHandler OnQueueFinished;

        /// <summary>
        /// Gets the amount of blocks queued
        /// </summary>
        public int BlocksQueued
        {
            get
            {
                return m_Queue.Count;
            }
        }

        /// <summary>
        /// Gets whether the uploader is uploading
        /// </summary>
        public bool Uploading
        {
            get
            {
                if (m_uploadThread == null)
                {
                    return false;
                }

                return m_uploadThread.ThreadState == ThreadState.Running;
            }
        }

        /// <summary>
        /// Waits for the blocks to be sent
        /// </summary>
        public void WaitForBlocks()
        {
            if (m_ResetEvent != null)
            {
                m_ResetEvent.WaitOne();
                m_ResetEvent.Dispose();
                m_ResetEvent = null;
            }
        }

        /// <summary>
        /// Waits for the blocks to be sent asynchronously
        /// </summary>
        /// <returns>The asynchronous task</returns>
        public Task WaitForBlocksAsync()
        {
            return Task.Run(() => WaitForBlocks());
        }

        /// <summary>
        /// Gets the next in the list
        /// </summary>
        /// <returns>The next block that should be sent</returns>
        private BlockRequest GetNextInList()
        {
            long oldestRequestTime = 0;
            BlockRequest oldestMissed = null;
            lock (m_Queue)
            {
                for (int i = 0; i < m_Queue.Count; i++)
                {
                    if (m_Queue[i] == null)
                    {
                        continue;
                    }

                    if (m_Queue[i].Missed)
                    {
                        long timePassed = m_Queue[i].GetTimePassed();
                        if (timePassed >= oldestRequestTime)
                        {
                            oldestRequestTime = timePassed;
                            oldestMissed = m_Queue[i];
                        }
                    }
                }
            }

            if (oldestMissed != null)
            {
                return oldestMissed;
            }

            lock (m_Queue)
            {
                //Check queue for the oldest unattempted send
                for (int i = 0; i < m_Queue.Count; i++)
                {
                    if (m_Queue[i] == null)
                    {
                        m_Queue.RemoveAt(i);
                        i--;
                        continue;
                    }

                    if (!m_Queue[i].HasBeenSent)
                    {
                        return m_Queue[i];
                    }
                }
            }

            return null;
        }

        private bool RemoveFromQueue(Block b)
        {
            lock (m_Queue)
            {
                for (int i = 0; i < m_Queue.Count; i++)
                {
                    if (m_Queue[i].Block.EqualsBlock(b))
                    {
                        m_Queue.RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The upload thread loop
        /// </summary>
        private void UploadThread()
        {
            try
            {
                while (m_Queue.Count > 0)
                {
                    if (!m_WorldConnection.Connected)
                    {
                        //Dont clear the queue or fire any events
                        m_TimeoutEvent.Dispose();
                        m_TimeoutEvent = null;
                        return;
                    }

                    //Send the most tasked block
                    BlockRequest send = GetNextInList();
                    if (send == null)
                    {
                        //We've sent all blocks that we've missed and all in the queue
                        //But it doesnt mean the queue is empty or is done yet
                        //Start waiting for block events for a certain delay, if none are received the requests
                        //were timed out and we need to resend
                        if (!m_TimeoutEvent.WaitOne(375))
                        {
                            //Blocks timedout set all queue blocks as missed
                            for (int i = 0; i < m_Queue.Count; i++)
                            {
                                m_Queue[i].SetMissed();
                            }
                        }

                        m_TimeoutEvent.Reset();
                        continue;
                    }
                    else if (send.Missed)
                    {
                        bool removed = false;
                        Block ex1 = m_WorldConnection.World[send.Block.X, send.Block.Y, send.Block.Layer];
                        if (ex1.EqualsBlock(send.Block))
                        {
                            RemoveFromQueue(send.Block);
                            continue;
                        }
                    }

                    if (!m_WorldConnection.World.CanPlace(send.Block))
                    {
                        RemoveFromQueue(send.Block);
                        continue;
                    }

                    send.Request();

                    Block ex2 = m_WorldConnection.World[send.Block.X, send.Block.Y, send.Block.Layer];
                    if (ex2.EqualsBlock(send.Block))
                    {
                        RemoveFromQueue(send.Block);
                        continue;
                    }

                    m_WorldConnection.UploadBlockRequest(send);
                }
            }
            catch (ThreadAbortException)
            {
                //Handled
            }
            catch (Exception ex)
            {
                m_WorldConnection.Client.Log.Add(FluidLogCategory.Fail, ex.Message);
            }

            if (m_TimeoutEvent != null)
            {
                //Dispose of timeout event
                m_TimeoutEvent.Dispose();
                m_TimeoutEvent = null;
            }

            //Clear the queue
            m_Queue.Clear();
            if (OnQueueFinished != null)
            {
                OnQueueFinished(this, EventArgs.Empty);
            }

            if (m_ResetEvent != null)
            {
                m_ResetEvent.Set();
            }
        }

        /// <summary>
        /// Gets if the queue is empty
        /// </summary>
        /// <returns></returns>
        internal bool IsQueueEmpty()
        {
            return m_Queue.Count == 0;
        }

        /// <summary>
        /// Gets if the upload thread is dead
        /// </summary>
        /// <returns></returns>
        internal bool IsUploadThreadDead()
        {
            if (m_uploadThread != null)
            {
                return m_uploadThread.ThreadState == ThreadState.Stopped;
            }

            return true;
        }

        /// <summary>
        /// Creates a new upload thread
        /// </summary>
        internal void CreateUploadThread()
        {
            m_uploadThread = new Thread(UploadThread);
            m_uploadThread.Priority = ThreadPriority.AboveNormal;
            m_uploadThread.IsBackground = true;
            m_uploadThread.Start();

            m_ResetEvent = new ManualResetEvent(false);
            m_TimeoutEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Stops the upload thread
        /// </summary>
        internal void StopThread()
        {
            if (Uploading)
            {
                m_uploadThread.Abort();
            }
        }

        /// <summary>
        /// Removes the expected block
        /// </summary>
        internal void RemoveExpected()
        {
            if (!IsQueueEmpty())
            {
                m_Queue.RemoveAt(0);
            }
        }

        /// <summary>
        /// Confirms a block was sent
        /// </summary>
        /// <param name="block">The block received from the server</param>
        internal void Confirm(Block block)
        {
            if (IsQueueEmpty())
            {
                return;
            }

            if (block.Placer != null)
            {
                if (block.Placer.Id == m_WorldConnection.Me.Id)
                {
                    if (m_TimeoutEvent != null)
                    {
                        m_TimeoutEvent.Set();
                    }

                    Block expected = m_Queue[0].Block;
                    if (!block.EqualsBlock(expected))
                    {
                        lock (m_Queue)
                        {
                            for (int i = 0; i < m_Queue.Count; i++)
                            {
                                if (m_Queue[i].Block.EqualsBlock(block))
                                {
                                    m_Queue.RemoveAt(i);
                                    break;
                                }
                                else
                                {
                                    m_Queue[i].SetMissed();
                                }
                            }
                        }
                    }
                    else
                    {
                        RemoveExpected();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the block to the end of the list
        /// </summary>
        /// <param name="block">The block to be queued</param>
        /// <param name="blockThrottle">The speed of the block to be uploaded at</param>
        internal void QueueBlock(Block block, int blockThrottle)
        {
            if (block == null)
            {
                return;
            }

            BlockRequest request = new BlockRequest(block, blockThrottle);
            m_Queue.Add(request);

            if (IsUploadThreadDead())
            {
                CreateUploadThread();
            }
        }

        /// <summary>
        /// Creates a new block manager
        /// </summary>
        /// <param name="worldCon"></param>
        internal BlockUploadManager(WorldConnection worldCon)
        {
            m_Queue = new List<BlockRequest>();
            m_WorldConnection = worldCon;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~BlockUploadManager()
        {
            if (Uploading)
            {
                StopThread();
            }

            if (m_ResetEvent != null)
            {
                m_ResetEvent.Dispose();
            }
        }
    }
}

using Fluid.Blocks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Fluid
{
    public class BlockUploadManager
    {
        private List<BlockRequest> m_Queue;
        private WorldConnection m_WorldConnection;
        private Thread m_uploadThread;
        private ManualResetEvent m_ResetEvent;

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
        /// Gets the next in the list
        /// </summary>
        /// <returns>The next block that should be sent</returns>
        private BlockRequest GetNextInList()
        {
            long oldestRequestTime = 0;
            BlockRequest oldestMissed = null;
            for (int i = 0; i < m_Queue.Count; i++)
            {
                if (m_Queue[i].Missed)
                {
                    if (!m_Queue[i].Timestamp.HasValue)
                    {
                        return m_Queue[i];
                    }

                    long timePassed = m_Queue[i].GetTimePassed();
                    if (timePassed >= oldestRequestTime)
                    {
                        oldestRequestTime = timePassed;
                        oldestMissed = m_Queue[i];
                    }
                }
            }

            if (oldestMissed != null)
            {
                return oldestMissed;
            }

            //Check queue for the oldest unattempted send
            for (int i = 0; i < m_Queue.Count; i++)
            {
                if (!m_Queue[i].HasBeenSent)
                {
                    return m_Queue[i];
                }
            }

            return null;
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
                    //Send the most tasked block
                    BlockRequest req = m_Queue[0];
                    BlockRequest send = GetNextInList();
                    if (send == null)
                    {
                        break;
                    }
                    
                    send.Request();
                    m_WorldConnection.UploadBlockRequest(send);
                }
            }
            catch (ThreadAbortException)
            {
                //Handled
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
                    Block expected = m_Queue[0].Block;

                    if (!block.EqualsBlock(expected))
                    {
                        for (int i = 0; i < m_Queue.Count; i++)
                        {
                            if (m_Queue[i].Block.EqualsBlock(block))
                            {
                                m_Queue.RemoveAt(i);
                                i--;
                            }
                            else
                            {
                                m_Queue[i].SetMissed();
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

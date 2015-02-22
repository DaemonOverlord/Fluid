using Fluid.Blocks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Fluid
{
    internal class BlockUploadManager
    {
        private List<BlockRequest> m_Queue;
        private WorldConnection m_WorldConnection;

        private Thread m_uploadThread;

        private void UploadThread()
        {
            while (m_Queue.Count > 0)
            {
                //Send the most tasked block
                BlockRequest req = m_Queue[0];
                BlockRequest send = null;

                if (req.GetTimePassed() > 0 && req.GetTimePassed() < 50)
                {
                    BlockRequest first = null;
                    for (int i = 0; i < m_Queue.Count; i++)
                    {
                        if (m_Queue[i].GetTimePassed() == 0)
                        {
                            first = m_Queue[i];
                            break;
                        }
                    }

                    if (first != null)
                    {
                        send = first;
                    }
                }
                else if (req.GetTimePassed() > 500)
                {
                    m_Queue.RemoveAt(0);
                    continue;
                }

                if (send == null)
                {
                    send = req;
                }

                send.Request();
                m_WorldConnection.UploadBlockRequest(send);
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

        internal void CreateUploadThread()
        {
            m_uploadThread = new Thread(UploadThread);
            m_uploadThread.Priority = ThreadPriority.AboveNormal;
            m_uploadThread.IsBackground = true;
            m_uploadThread.Start();
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

                    if (!block.Equals(expected))
                    {
                        for (int i = 0; i < m_Queue.Count; i++)
                        {
                            if (m_Queue[i].Block.Equals(block))
                            {
                                m_Queue.RemoveAt(i);
                                i--;
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
    }
}

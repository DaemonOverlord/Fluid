using System;
using System.Data;
using System.Data.SQLite;

namespace Fluid
{
    public class FluidPlayerDatabase
    {
        private FluidClient m_Client;
        private FluidLog m_Logger;
        private SQLiteConnection m_dbConnection;
        private const string m_UsernameQuery = "SELECT username FROM usernames WHERE userid = '{0}' LIMIT 1";
        private const string m_ConnectionIdQuery = "SELECT userid FROM usernames WHERE username = '{0}' LIMIT 1";

        /// <summary>
        /// Gets whether the connection is established
        /// </summary>
        public bool Connected { get { return m_dbConnection.State == ConnectionState.Open; } }

        /// <summary>
        /// Checks if the database connection is open
        /// </summary>
        private bool CheckIfOpen()
        {
            if (m_dbConnection.State != ConnectionState.Open)
            {
                if (m_Logger != null)
                {
                    m_Logger.Add(FluidLogCategory.Fail, "Fluid could not load data from the database because it is not open yet, consider opening the database before querying.");
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Executes a sql lite query
        /// </summary>
        /// <param name="commandText">The command text</param>
        private object ExecuteQuery(string commandText)
        {
            if (CheckIfOpen())
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = commandText;

                try
                {
                    return command.ExecuteScalar();
                }
                catch
                {
                    m_Logger.Add(FluidLogCategory.Fail, "Corrupt player database. Failed to interact with database.");
                }
            }

            return null;
        }

        /// <summary>
        /// Executes a sql lite command
        /// </summary>
        /// <param name="commandText">The command text</param>
        private void ExecuteNonQuery(string commandText)
        {
            if (CheckIfOpen())
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = commandText;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    m_Logger.Add(FluidLogCategory.Fail, "Corrupt player database. Failed to interact with database.");
                }            
            }
        }

        /// <summary>
        /// Looks up the username to a corresponding connection id
        /// </summary>
        /// <param name="connectionId">The connection id</param>
        /// <returns>The username if found; otherwise null</returns>
        public string GetUsername(string connectionId)
        {
            string commandText = string.Format(m_UsernameQuery, connectionId);
            object result = ExecuteQuery(commandText);
            if (result == null)
            {
                return null;
            }

            return (string)result;
        }

        /// <summary>
        /// Adds a entry to the database
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="connectionId">The connection id</param>
        public void Add(string username, string connectionId)
        {
            ExecuteNonQuery(string.Format("insert into usernames(username, userid) values('{0}', '{1}');", username, connectionId));
        }

        /// <summary>
        /// Sets the value in the database
        /// </summary>
        /// <param name="connectionId">The connection id of the user to change</param>
        /// <param name="username">The new username</param>
        public void Set(string connectionId, string username)
        {
            ExecuteNonQuery(string.Format("update usernames SET username='{0}' where userid='{1}';", username, connectionId));
        }

        /// <summary>
        /// Looks up the connection id to a corresponding username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The connection id if found; otherwise null</returns>
        public string GetConnectionId(string username)
        {
            string commandText = string.Format(m_ConnectionIdQuery, username);
            object result = ExecuteQuery(commandText);
            if (result == null)
            {
                if (m_Client.Config.AddProfilesToDatabase)
                {
                    Profile profile = m_Client.LoadProfile(username);
                    if (profile != null)
                    {
                        this.Add(username, profile.ConnectionId);
                        return profile.ConnectionId;
                    }
                }
                else
                {
                    m_Logger.Add(FluidLogCategory.Message, "Consider setting AddProfilesToDatabase to true in your config, this will automatically add unknown usernames to the database.");
                }
                return null;
            }

            return (string)result;
        }

        /// <summary>
        /// Connects to the player id database
        /// </summary>
        /// <param name="toolBelt">The Fluid toolbelt</param>
        public void Connect(FluidToolbelt toolBelt)
        {
            if (toolBelt == null)
            {
                if (m_Logger != null)
                {
                    m_Logger.Add(FluidLogCategory.Fail, "Cannot connect to the database because you passed in a null FluidToolbelt");
                }

                return;
            }

            if (!toolBelt.RunSafe(delegate() { m_dbConnection.Open(); }) && m_Logger != null)
            {
                m_Logger.Add(FluidLogCategory.Fail, "Fluid could not load local database, please make sure the local database is in the same directory as Fluid.dll");
            }
        }

        /// <summary>
        /// Creates a new Fluid player database
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="logger">The Fluid logger</param>
        public FluidPlayerDatabase(FluidClient client, FluidLog logger)
        {
            this.m_Client = client;
            this.m_Logger = logger;
            this.m_dbConnection = new SQLiteConnection("Data Source=gDat.db");
        }
    }
}

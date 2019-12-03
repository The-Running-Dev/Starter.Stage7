using System;
using System.Data;

namespace Starter.Framework.Extensions
{
    /// <summary>
    /// Extension methods to the IDbCommand type
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a list of dictionary parameters to the IDbCommand
        /// </summary>
        /// <param name="command">The command to use</param>
        /// <param name="parameters">The parameters to add</param>
        public static void AddParameters(this IDbCommand command, IDbDataParameter[] parameters = null)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var p in parameters)
            {
                command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// Adds a new input parameter and it's value to IDbCommand
        /// </summary>
        /// <param name="command">The command to use</param>
        /// <param name="parameter"></param>
        //public static void AddParameter(this IDbCommand command, IDbDataParameter parameter)
        //{
        //    if (command == null) return;

        //    var p = command.CreateParameter();

        //    parameter.ParameterName = parameter.ParameterName;
        //    parameter.Value = parameter.Value;
        //    parameter.Direction = ParameterDirection.Input;

        //    command.Parameters.Add(p);

        //    command.Parameters.Add(parameter);
        //}
    }
}
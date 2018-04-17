using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Models
{
    /// <summary>
    /// Model for storing standard input streamwriter
    /// </summary>
    public class StreamWriterModel
    {
        /// <summary>
        /// Id of associated process
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// StreamWriter instance
        /// </summary>
        public StreamWriter StreamWriter { get; set; }
    }
}

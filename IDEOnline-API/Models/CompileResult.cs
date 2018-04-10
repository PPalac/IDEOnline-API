namespace IDEOnlineAPI.Models
{
    /// <summary>
    /// Viewmodel to pass compile result to forntend.
    /// </summary>
    public class CompileResult
    {
        /// <summary>
        /// Information about compilation output.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Unique ID. Used to run compiled console application.
        /// </summary>
        public string ID { get; set; }
    }
}

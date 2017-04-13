namespace Ndx.Tools.Metacap
{
    public enum ProgressRecordType { Processing, Completed }

    /// <summary>
    /// Represent the status of an ongoing operation at a point in time.B
    /// </summary>
    public class ProgressRecord
    {
        private int m_activityId;
        private string m_activity;
        private string m_statusDescription;

        /// <summary>
        /// Gets or sets the description of the activity for which progress is being reported (for example, "Recursively removing item c:\temp.".).
        /// </summary>
        public string Activity { get => m_activity ; set => m_activity = value; }

        /// <summary>
        /// Gets the identifier of the activity to which this record corresponds.
        /// </summary>
        public int ActivityId { get => m_activityId; }

        /// <summary>
        /// Gets or sets the current operation of the activity (for example, "copying my.txt").
        /// </summary>
        public string CurrentOperation { get; set; }

        /// <summary>
        /// Gets or sets an estimate of the percentage of total work that is completed for the activity.
        /// </summary>
        public int PercentComplete { get; set; }

        /// <summary>
        ///  Gets or sets the overall status (Processing or Completed) of the activity.
        /// </summary>
        public ProgressRecordType RecordType { get; set; }


        /// <summary>
        /// Gets or sets an estimate of time that remains until this activity is completed.
        /// </summary>
        public int SecondsRemaining { get; set; }


        /// <summary>
        /// Gets or sets a description of the current status of the activity.
        /// </summary>
        public string StatusDescription { get => m_statusDescription; set => m_statusDescription = value; }

        /// <summary>
        /// Initializes a new instance of the ProgressRecord class that contains the current activity identifier, a description of the current activity, and a description of the status.
        /// </summary>
        /// <param name="activityId">A unique numeric key that identifies the activity for which progress is being reported.</param>
        /// <param name="activity">A description of the activity that is being performed.</param>
        /// <param name="statusDescription">A description of the status of the activity.</param>
        public ProgressRecord(int activityId, string activity, string statusDescription)
        {
            m_activityId = activityId;
            m_activity = activity;
            m_statusDescription = statusDescription;
        }
    }
}

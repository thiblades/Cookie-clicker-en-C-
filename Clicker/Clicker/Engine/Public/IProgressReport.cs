using System;

namespace Clicker.Engine.Public {
    /// <summary>
    /// Interface used by long-running operations to report their progress.
    /// </summary>
    /// 
    /// Objects that comply to this interface are passed to long running operations
    /// such as scene and game loading so that they can report on their progress.
    /// This progress is then presented to the user, typically within a loading
    /// screen.
    public interface IProgressReport {
        /// <summary>
        /// Report the current progress.
        /// </summary>
        /// <param name="progress">Completion ratio, comprised within [0;1].</param>
        /// <param name="message">Message indicating what is being done.</param>
        void ReportProgress(float progress, String message = null);
    }
}

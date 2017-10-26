using System;

namespace Clicker.Engine.Public {
    public interface IProgressReport {
        void ReportProgress(float progress, String message = null);
    }
}

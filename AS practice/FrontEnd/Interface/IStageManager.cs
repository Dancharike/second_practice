using System.Windows.Forms;

namespace AS_practice.Interface
{
    public interface IStageManager
    {
        void LoadStage(MainForm mainForm, Panel parentPanel, object context = null);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class UserEvent
    {
        public delegate void Button_Click(object sender, EventArgs e);
        public static event Button_Click renovateBtn_Click;

        public delegate void BtnAutoRun(object sender, EventArgs e);
        public static event BtnAutoRun btnAutoRun;

        public delegate void StaticalWndUI();
        public static event StaticalWndUI staticalWndUI;

        public delegate void ProductChange();
        public static event ProductChange productChange;

        public delegate void SaveSmtBurnLog();
        public static event SaveSmtBurnLog saveSmtBurnLog;

        public delegate void GetMesInfo();
        public static event GetMesInfo getMesInfo;

        public delegate bool StartMesSystem();
        public static event StartMesSystem startMesSystem;

        public delegate void InitMesUI();
        public static event InitMesUI initMesUI;

        public delegate void GetMesDate();
        public static event GetMesDate getMesDate;

        public delegate void UpdateControls();
        public static event UpdateControls updateControls;

        public delegate void EnableSeat(int Group, int Unit);
        public static event EnableSeat enableSeat;

        public void RenovateBtn_Click()
        {
            if (renovateBtn_Click != null)
            {
                renovateBtn_Click(this, new EventArgs());
            }
        }

        public void BtnAutoRun_Click()
        {
            if (btnAutoRun != null)
            {
                btnAutoRun(this, new EventArgs());
            }
        }

        public void StaticalWndUI_Click()
        {
            if (staticalWndUI != null)
            {
                staticalWndUI();
            }
        }

        public void ProductChange_Click()
        {
            if (productChange != null)
            {
                productChange();
            }
        }

        public void SaveSmtBurnLog_Click()
        {
            saveSmtBurnLog?.Invoke();
        }

        public void GetMesInfo_Click()
        {
            getMesInfo?.Invoke();
        }

        public bool StartMesSystem_Click()
        {
            if (startMesSystem?.Invoke() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InitMesUI_Click()
        {
            initMesUI?.Invoke();
        }

        public void GetMesDate_Click()
        {
            getMesDate?.Invoke();
        }

        public void UpdateControls_Click()
        {
            updateControls?.Invoke();
        }

        public void EnableSeat_Click(int Group, int Unit)
        {
            enableSeat?.Invoke(Group, Unit);
        }
    }
}

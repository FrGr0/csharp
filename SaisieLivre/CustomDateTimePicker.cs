using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace CustomDateTimePicker
{
    class CustomDateTimePicker
    {
        public class DPDateTimePicker : DateTimePicker
        {            
            private bool selectionComplete = false;
            private bool numberKeyPressed = false;

            private const int WM_KEYUP = 0x0101;
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_REFLECT = 0x2000;
            private const int WM_NOTIFY = 0x004e;

            /*
            public DPDateTimePicker() : base()
            {
                this.SetStyle(ControlStyles.UserPaint, true);
            }
            
            public override Color BackColor
            {
                get { return base.BackColor; }
                set { base.BackColor = value; }
            }

            public override Color ForeColor
            {
                get { return base.ForeColor; }
                set { base.ForeColor = value; }
            }
            */
            
            [StructLayout(LayoutKind.Sequential)]
            private struct NMHDR
            {
                public IntPtr hwndFrom;
                public IntPtr idFrom;
                public int Code;
            }

            /*
            protected override void OnPaint(PaintEventArgs e)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment = StringAlignment.Center;
                using (Brush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);
                }
                using (Brush brush = new SolidBrush(ForeColor))
                {
                    e.Graphics.DrawString(Text, Font, brush, ClientRectangle, stringFormat);
                }
                Rectangle dropDownRectangle = new Rectangle(
                    ClientRectangle.Width - Height, 0, Height, Height);

                if (ComboBoxRenderer.IsSupported)
                    ComboBoxRenderer.DrawDropDownButton(e.Graphics, dropDownRectangle, ComboBoxState.Normal);
                
                else
                    ControlPaint.DrawComboButton(e.Graphics, dropDownRectangle, ButtonState.Flat);
            } */


            protected override void OnKeyDown(KeyEventArgs e)
            {
                numberKeyPressed = (e.Modifiers == Keys.None && ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode != Keys.Back && e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)));
                selectionComplete = false;
                base.OnKeyDown(e);
            }

            
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_REFLECT + WM_NOTIFY)
                {
                    var hdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                    if (hdr.Code == -759) //date chosen (by keyboard)
                        selectionComplete = true;
                }
                base.WndProc(ref m);
            }
            

            protected override void OnKeyUp(KeyEventArgs e)
            {
                base.OnKeyUp(e);
                if (numberKeyPressed && selectionComplete &&
                    (e.Modifiers == Keys.None && ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode != Keys.Back && e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))))
                {
                    Message m = new Message();
                    m.HWnd = this.Handle;
                    m.LParam = IntPtr.Zero;
                    m.WParam = new IntPtr((int)Keys.Right); //right arrow key
                    m.Msg = WM_KEYDOWN;
                    base.WndProc(ref m);
                    m.Msg = WM_KEYUP;
                    base.WndProc(ref m);
                    numberKeyPressed = false;
                    selectionComplete = false;
                }
            }

        }
    }
}

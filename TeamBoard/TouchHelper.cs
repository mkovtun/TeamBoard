using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Team_board
{
    public class TouchHelper
    {
        public enum TOUCH_MASK : uint
        {
            TOUCH_MASK_NONE = 0x00000000,
            TOUCH_MASK_CONTACTAREA = 0x00000001,
            TOUCH_MASK_ORIENTATION = 0x00000002,
            TOUCH_MASK_PRESSURE = 0x00000004
        }
        public enum POINTER_INPUT_TYPE : uint
        {
            PT_POINTER = 0x00000001,
            PT_TOUCH = 0x00000002,
            PT_PEN = 0x00000003,
            PT_MOUSE = 0x00000004
        }

        public enum POINTER_FLAGS : uint
        {
            POINTER_FLAG_NONE = 0x00000000,
            POINTER_FLAG_NEW = 0x00000001,
            POINTER_FLAG_INRANGE = 0x00000002,
            POINTER_FLAG_INCONTACT = 0x00000004,
            POINTER_FLAG_FIRSTBUTTON = 0x00000010,
            POINTER_FLAG_SECONDBUTTON = 0x00000020,
            POINTER_FLAG_THIRDBUTTON = 0x00000040,
            POINTER_FLAG_OTHERBUTTON = 0x00000080,
            POINTER_FLAG_PRIMARY = 0x00000100,
            POINTER_FLAG_CONFIDENCE = 0x00000200,
            POINTER_FLAG_CANCELLED = 0x00000400,
            POINTER_FLAG_DOWN = 0x00010000,
            POINTER_FLAG_UPDATE = 0x00020000,
            POINTER_FLAG_UP = 0x00040000,
            POINTER_FLAG_WHEEL = 0x00080000,
            POINTER_FLAG_HWHEEL = 0x00100000
        }
        public enum TOUCH_FEEDBACK : uint
        {
            TOUCH_FEEDBACK_DEFAULT = 0x1,
            TOUCH_FEEDBACK_INDIRECT = 0x2,
            TOUCH_FEEDBACK_NONE = 0x3
        }

        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InjectTouch(int x, int y, POINTER_INPUT_TYPE pt_input, int pressure, int orientation, int id, int rcContactTop, int rcContactBottom, int rcContactLeft, int rcContactRight, POINTER_FLAGS pointerFlags, TOUCH_MASK touchMask);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setTouchFeedback(TOUCH_FEEDBACK fb);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultRectSize(int size);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultPressure(int pres);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultOrientation(int or);

        [DllImport("User32.dll")]
        static extern Boolean MessageBeep(UInt32 beepType);

        public static void mouseclick(int x, int y)
        {
            bool ret;
            setTouchFeedback(TOUCH_FEEDBACK.TOUCH_FEEDBACK_INDIRECT);
            ret = InjectTouch(x, y, POINTER_INPUT_TYPE.PT_TOUCH, 3200, 0, 0, x - 4, x + 4, y - 4, y + 4, POINTER_FLAGS.POINTER_FLAG_DOWN | POINTER_FLAGS.POINTER_FLAG_INCONTACT | POINTER_FLAGS.POINTER_FLAG_INRANGE, TOUCH_MASK.TOUCH_MASK_CONTACTAREA | TOUCH_MASK.TOUCH_MASK_ORIENTATION | TOUCH_MASK.TOUCH_MASK_PRESSURE);
            if (ret)
            {
                ret = InjectTouch(x, y, POINTER_INPUT_TYPE.PT_TOUCH, 3200, 0, 0, x - 4, x + 4, y - 4, y + 4, POINTER_FLAGS.POINTER_FLAG_UP, TOUCH_MASK.TOUCH_MASK_CONTACTAREA | TOUCH_MASK.TOUCH_MASK_ORIENTATION | TOUCH_MASK.TOUCH_MASK_PRESSURE);
            }
            else
            {
                MessageBeep(0);
            }
        } 
    }
}

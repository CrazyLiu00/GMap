//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Alt.Sketch;
using Alt.Threading;


namespace Alt.PCL
{
    class PCLThread : IPCLThread
    {
        public PCLThread(System.Threading.Thread thread)
            : base(thread)
        {
        }


        public override int ManagedThreadId
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.ManagedThreadId;
                }

                return -1;
            }
        }


        public override CultureInfo CurrentCulture
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.CurrentCulture;
                }

                return CultureInfo.CurrentCulture;
            }
            set
            {
                if (IsThreadValid)
                {
                    THREAD.CurrentCulture = value;
                }
            }
        }


        public override CultureInfo CurrentUICulture
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.CurrentUICulture;
                }

                return CultureInfo.CurrentUICulture;
            }
            set
            {
                if (IsThreadValid)
                {
                    THREAD.CurrentUICulture = value;
                }
            }
        }


        public override bool IsAlive
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.IsAlive;
                }

                return false;
            }
        }


        public override bool IsBackground
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.IsBackground;
                }

                return false;
            }
            set
            {
                if (IsThreadValid)
                {
                    THREAD.IsBackground = value;
                }
            }
        }


        public override string Name
        {
            get
            {
                if (IsThreadValid)
                {
                    return THREAD.Name;
                }

                return string.Empty;
            }
            set
            {
                if (IsThreadValid)
                {
                    THREAD.Name = value;
                }
            }
        }


        public override ThreadPriority Priority
        {
            get
            {
                if (IsThreadValid)
                {
#if !SILVERLIGHT
                    return PCLTools.ToThreadPriority(THREAD.Priority);
#endif
                }

                return ThreadPriority.Normal;
            }
            set
            {
                if (IsThreadValid)
                {
#if !SILVERLIGHT
                    THREAD.Priority = PCLTools.ToThreadPriority(value);
#endif
                }
            }
        }


        public override ThreadState ThreadState
        {
            get
            {
                if (IsThreadValid)
                {
                    return PCLTools.ToThreadState(THREAD.ThreadState);
                }

                return ThreadState.Unstarted;
            }
        }


        public override void Abort()
        {
            if (IsThreadValid)
            {
                THREAD.Abort();
            }
        }
        

        public override void Interrupt()
        {
            if (IsThreadValid)
            {
                THREAD.Interrupt();
            }
        }


        public override bool Join(int millisecondsTimeout)
        {
            if (IsThreadValid)
            {
                return THREAD.Join(millisecondsTimeout);
            }

            return true;
        }


        public override void Start()
        {
            if (IsThreadValid)
            {
                THREAD.Start();
            }
        }


        public override void Start(object parameter)
        {
            if (IsThreadValid)
            {
                THREAD.Start(parameter);
            }
        }


        public override ApartmentState GetApartmentState()
        {
            if (IsThreadValid)
            {
#if !SILVERLIGHT
                return PCLTools.ToApartmentState(THREAD.GetApartmentState());
#endif
            }

            return ApartmentState.Unknown;
        }

        public override void SetApartmentState(ApartmentState state)
        {
            if (IsThreadValid)
            {
#if !SILVERLIGHT
                THREAD.SetApartmentState(PCLTools.ToApartmentState(state));
#endif
            }
        }
    }
}

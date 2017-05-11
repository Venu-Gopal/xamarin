using System;
using System.Collections.Generic;
using System.Text;

namespace CMSapp.PushNotification
{
    interface IPushNotificationRegister
    {
        void ExtractTokenAndRegister();
    }
}

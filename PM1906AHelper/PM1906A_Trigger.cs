﻿using PM1906AHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM1906AHelper
{
    public partial class PM1906A
    {
        /// <summary>
        /// Start to response the trigger signals.
        /// </summary>
        public void Trigger_Start()
        {
            _write(CMD_TS_START);
        }

        /// <summary>
        /// Stop to response the trigger signals.
        /// </summary>
        public void Trigger_Stop()
        {
            _write(CMD_TS_STOP);
        }

        /// <summary>
        /// Get the status of the trigger.
        /// <para><see cref="ArgumentException"/> will be returned if the return string is not IDLE or BUSY.</para>
        /// </summary>
        /// <returns></returns>
        public TriggerStatusEnum Trigger_GetStatus()
        {
            var ret = _query($"{CMD_TS_STA}?");
            if (ret.ToUpper() == "IDLE")
                return TriggerStatusEnum.IDLE;
            else if (ret.ToUpper() == "BUSY")
                return TriggerStatusEnum.BUSY;
            else
                throw new FormatException($"the return value {ret} is incorrect.");
        }

        /// <summary>
        /// Clean the used trigger buffer.
        /// </summary>
        public void Trigger_CleanBuffer()
        {
            _write(CMD_TS_CLR);
        }

        /// <summary>
        /// Get how many optical power are there in the trigger buffer.
        /// </summary>
        /// <returns></returns>
        public int Trigger_GetUsedBuffLen()
        {
            var ret = _query($"{CMD_TS_GET_LEN}?");
            if (int.TryParse(ret, out int len))
                return len;
            else
                throw new FormatException($"the return value {ret} is incorrect.");
        }
    }
}

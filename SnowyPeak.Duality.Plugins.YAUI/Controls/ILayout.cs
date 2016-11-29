using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
    public interface ILayout
    {
        void LayoutControls();

        Control FindHoveredControl(Vector2 position);
    }
}
using IHI.Server.Plugins;
using IHI.Server.WebAdmin;

namespace IHI.Server.Plugins.Cecer1.StandardOut
{
    public class WEB_StandardOut : Plugin
    {
        Handlers fHandlers;

        public override void Start()
        {
            this.fHandlers = new Handlers(this);

            CoreManager.GetCore().GetWebAdminManager().AddPathHandler("/admin/stdout", new HttpPathHandler(this.fHandlers.PAGE_Index));
            CoreManager.GetCore().GetWebAdminManager().AddPathHandler("/admin/stdout/send", new HttpPathHandler(this.fHandlers.PAGE_Message));
            CoreManager.GetCore().GetWebAdminManager().AddPathHandler("/admin/stdout/update", new HttpPathHandler(this.fHandlers.PAGE_Update));
        }

        public override void Stop()
        {
            CoreManager.GetCore().GetWebAdminManager().RemovePathHandler("/admin/stdout");
            CoreManager.GetCore().GetWebAdminManager().RemovePathHandler("/admin/stdout/send");
            CoreManager.GetCore().GetWebAdminManager().RemovePathHandler("/admin/stdout/update");

            this.fHandlers = null;
        }
    }
}
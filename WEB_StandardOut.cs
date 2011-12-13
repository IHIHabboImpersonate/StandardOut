using IHI.Server.WebAdmin;

namespace IHI.Server.Plugins.Cecer1.StandardOut
{
    public class WEB_StandardOut : Plugin
    {
        private Handlers _handlers;

        public override void Start()
        {
            _handlers = new Handlers(this);
            WebAdminManager webAdminManager = CoreManager.ServerCore.GetWebAdminManager();

            webAdminManager.AddPathHandler("/admin/stdout", _handlers.PageIndex);
            webAdminManager.AddPathHandler("/admin/stdout/send", _handlers.PageMessage);
            webAdminManager.AddPathHandler("/admin/stdout/update", _handlers.PageUpdate);
        }
    }
}
namespace IHI.Server.Plugins.Cecer1.StandardOut
{
    public class WEB_StandardOut : Plugin
    {
        private Handlers _handlers;

        public override void Start()
        {
            _handlers = new Handlers(this);
            var webAdminManager = CoreManager.GetServerCore().GetWebAdminManager();

            webAdminManager.AddPathHandler("/admin/stdout", _handlers.PageIndex);
            webAdminManager.AddPathHandler("/admin/stdout/send", _handlers.PageMessage);
            webAdminManager.AddPathHandler("/admin/stdout/update", _handlers.PageUpdate);
        }
    }
}
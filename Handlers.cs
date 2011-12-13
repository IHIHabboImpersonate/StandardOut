using System.Net;
using IHI.Server.WebAdmin;

namespace IHI.Server.Plugins.Cecer1.StandardOut
{
    internal class Handlers
    {
        private readonly Plugin _plugin;

        internal Handlers(Plugin owner)
        {
            _plugin = owner;
        }

        internal void PageIndex(HttpListenerContext context)
        {
            if (context.Request.IsLocal)
            {
                WebAdminManager.SendResponse(context.Response, _plugin.GetName(),
                                             @"<!DOCTYPE html>
<html>
	<head>
		<title>IHI Web Admin Example</title>
		<style type='text/css'>
			body
			{
				background-color: #DDD;
			}
		</style>
		<script type='text/javascript' src='http://code.jquery.com/jquery-1.6.1.min.js'></script>
		<script type='text/javascript'>
			function Send(event)
			{
				if(event.keyCode == 13)
				{
					jQuery.get('/admin/stdout/send?level=' + $('#importancea')[0].value + '&data=' + $('#textbox')[0].value);
				}
			}
			function Update()
			{
			    jQuery.get('/admin/stdout/update?level=' + $('#importanceb')[0].value);
			}
		</script>
	</head>
	<body>
        <b>Send</b>
		<select id='importancea'>
			<option value='0'>Debug</option>
			<option value='1'>Notice</option>
			<option value='2'>Important</option>
			<option value='3'>Warning</option>
			<option value='4'>Error</option>
		</select>
		<input id='textbox' type='textbox' size='100' onkeydown='Send(event)'><br>
        <br>
        <b>Importance Filter</b>
        <select id='importanceb' onchange='Update()'>
			<option value='0'>Debug</option>
			<option value='1'>Notice</option>
			<option value='2'>Important</option>
			<option value='3'>Warning</option>
			<option value='4'>Error</option>
		</select>
	</body>
</html>");
            }
        }

        internal void PageMessage(HttpListenerContext context)
        {
            if (!context.Request.IsLocal) return;
            string level = context.Request.QueryString["level"];
            string data = context.Request.QueryString["data"];
            byte levelByte;

            if (byte.TryParse(level, out levelByte))
            {
                switch (levelByte)
                {
                    case 0:
                        CoreManager.ServerCore.GetStandardOut().PrintDebug(data);
                        break;
                    case 1:
                        CoreManager.ServerCore.GetStandardOut().PrintNotice(data);
                        break;
                    case 2:
                        CoreManager.ServerCore.GetStandardOut().PrintImportant(data);
                        break;
                    case 3:
                        CoreManager.ServerCore.GetStandardOut().PrintWarning(data);
                        break;
                    case 4:
                        CoreManager.ServerCore.GetStandardOut().PrintError(data);
                        break;
                    default:
                        {
                            WebAdminManager.SendResponse(context.Response,
                                                         _plugin.GetName(),
                                                         "FAILURE");
                            return;
                        }
                }
                WebAdminManager.SendResponse(context.Response, _plugin.GetName(),
                                             "SUCCESS");
                return;
            }
            WebAdminManager.SendResponse(context.Response, _plugin.GetName(),
                                         "FAILURE");
        }

        internal void PageUpdate(HttpListenerContext context)
        {
            if (!context.Request.IsLocal) return;
            string level = context.Request.QueryString["level"];
            byte levelByte;

            if (byte.TryParse(level, out levelByte))
            {
                if (levelByte <= 4)
                {
                    CoreManager.ServerCore.GetStandardOut().SetImportance((StandardOutImportance) levelByte);
                    WebAdminManager.SendResponse(context.Response, _plugin.GetName(),
                                                 "SUCCESS");
                    return;
                }
            }
            WebAdminManager.SendResponse(context.Response, _plugin.GetName(),
                                         "FAILURE");
        }
    }
}
using System.Net;
using IHI.Server.WebAdmin;

namespace IHI.Server.Plugins.Cecer1.StandardOut
{
    internal class Handlers
    {
        Plugin fOwner;
        internal Handlers(Plugin Owner)
        {
            this.fOwner = Owner;
        }

        internal void PAGE_Index(HttpListenerContext Context)
        {
            if (Context.Request.IsLocal)
            {
                CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(),
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
        internal void PAGE_Message(HttpListenerContext Context)
        {
            if (Context.Request.IsLocal)
            {
                string Level = Context.Request.QueryString["level"];
                string Data = Context.Request.QueryString["data"];
                byte L = 0;

                if (byte.TryParse(Level, out L))
                {
                    switch (L)
                    {
                        case 0:
                            CoreManager.GetCore().GetStandardOut().PrintDebug(Data);
                            break;
                        case 1:
                            CoreManager.GetCore().GetStandardOut().PrintNotice(Data);
                            break;
                        case 2:
                            CoreManager.GetCore().GetStandardOut().PrintImportant(Data);
                            break;
                        case 3:
                            CoreManager.GetCore().GetStandardOut().PrintWarning(Data);
                            break;
                        case 4:
                            CoreManager.GetCore().GetStandardOut().PrintError(Data);
                            break;
                        default:
                            {
                                CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(), "FAILURE");
                                return;
                            }
                    }
                    CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(), "SUCCESS");
                    return;
                }
                CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(), "FAILURE");
            }
        }

        internal void PAGE_Update(HttpListenerContext Context)
        {
            if (Context.Request.IsLocal)
            {
                string Level = Context.Request.QueryString["level"];
                byte L = 0;

                if (byte.TryParse(Level, out L))
                {
                    if(L <= 4)
                    {
                        CoreManager.GetCore().GetStandardOut().SetImportance((StandardOutImportance)L);
                        CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(), "SUCCESS");
                        return;
                    }
                }
                CoreManager.GetCore().GetWebAdminManager().SendResponse(Context.Response, this.fOwner.GetName(), "FAILURE");
            }
        }
    }
}
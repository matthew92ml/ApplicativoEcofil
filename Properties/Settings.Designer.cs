using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string password
	{
		get
		{
			return (string)this["password"];
		}
		set
		{
			this["password"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string username
	{
		get
		{
			return (string)this["username"];
		}
		set
		{
			this["username"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool updateData
	{
		get
		{
			return (bool)this["updateData"];
		}
		set
		{
			this["updateData"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int timeSeconds
	{
		get
		{
			return (int)this["timeSeconds"];
		}
		set
		{
			this["timeSeconds"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string AlarmClock
	{
		get
		{
			return (string)this["AlarmClock"];
		}
		set
		{
			this["AlarmClock"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool Active
	{
		get
		{
			return (bool)this["Active"];
		}
		set
		{
			this["Active"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int Min
	{
		get
		{
			return (int)this["Min"];
		}
		set
		{
			this["Min"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int Max
	{
		get
		{
			return (int)this["Max"];
		}
		set
		{
			this["Max"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool ActivePS
	{
		get
		{
			return (bool)this["ActivePS"];
		}
		set
		{
			this["ActivePS"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool updateData2
	{
		get
		{
			return (bool)this["updateData2"];
		}
		set
		{
			this["updateData2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int timeMinutes
	{
		get
		{
			return (int)this["timeMinutes"];
		}
		set
		{
			this["timeMinutes"] = value;
		}
	}

	private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
	{
	}

	private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
	{
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
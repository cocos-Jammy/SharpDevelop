﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the BSD license (for details please see \src\AddIns\Debugger\Debugger.AddIn\license.txt)

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using Debugger;
using ICSharpCode.Core;

namespace ICSharpCode.SharpDevelop.Services
{
	public enum ShowIntegersAs { Decimal, Hexadecimal, Both };
	
	[Serializable]
	public class DebuggingOptions: Options
	{
		public static DebuggingOptions Instance {
			get {
				return PropertyService.Get("DebuggingOptions", new DebuggingOptions());
			}
		}
		
		public DebuggingOptions()
		{
			ShowIntegersAs = ShowIntegersAs.Decimal;
			DebuggerEventWindowState = FormWindowState.Normal;
			DebuggeeExceptionWindowState = FormWindowState.Normal;
		}
		
		public ShowIntegersAs ShowIntegersAs { get; set; }
		public bool ShowArgumentNames { get; set; }
		public bool ShowArgumentValues { get; set; }
		public bool ShowExternalMethods { get; set; }
		public bool ShowLineNumbers { get; set; }
		public bool ShowModuleNames { get; set; }
		
		// Properties for the DebuggerExceptionForm
		public FormWindowState DebuggerEventWindowState { get; set; }
		
		// Properties for the DebuggeeExceptionForm
		public FormWindowState DebuggeeExceptionWindowState { get; set; }
		
		/// <summary>
		/// Used to update status of some debugger properties while debugger is running.
		/// </summary>
		internal static void ResetStatus(Action<Process> resetStatus)
		{
			Process proc = WindowsDebugger.CurrentProcess;
			// debug session is running
			if (proc != null) {
				bool wasPausedNow = false;
				// if it is not paused, break execution
				if (!proc.IsPaused) {
					proc.Break();
					wasPausedNow = true;
				}
				resetStatus(proc);
				// continue if it was not paused before
				if (wasPausedNow)
					proc.AsyncContinue();
			}
		}
	}
}

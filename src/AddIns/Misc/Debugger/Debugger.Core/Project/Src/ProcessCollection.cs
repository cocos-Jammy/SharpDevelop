﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;
using Debugger.Interop.CorDebug;

namespace Debugger
{
	public class ProcessCollection: CollectionWithEvents<Process>
	{
		public ProcessCollection(NDebugger debugger): base(debugger) {}
		
		internal Process this[ICorDebugProcess corProcess] {
			get {
				foreach (Process process in this) {
					if (process.CorProcess == corProcess) {
						return process;
					}
				}
				return null;
			}
		}

		protected override void OnRemoved(Process item)
		{
			base.OnRemoved(item);
			
			if (this.Count == 0) {
				// Exit callback and then terminate the debugger
				this.Debugger.MTA2STA.AsyncCall( delegate { this.Debugger.TerminateDebugger(); } );
			}
		}
	}
}

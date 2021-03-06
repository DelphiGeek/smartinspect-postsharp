﻿/**
 * Copyright Gurock Software GmbH. All rights reserved.
 * http://www.gurock.com - contact@gurock.com
 *
 * Learn more about the SmartInspect for PostSharp aspects on
 * the following project website:
 *
 * http://code.gurock.com/p/smartinspect-postsharp/
 */

using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Gurock.SmartInspect.PostSharp
{
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[AspectRoleDependency(AspectDependencyAction.Order,
		AspectDependencyPosition.After, StandardRoles.Tracing)]
	[AspectTypeDependency(AspectDependencyAction.Commute,
		typeof(SiFieldAttribute))]
	public class SiExceptionAttribute: SiBaseAttribute
	{
		private bool m_IncludeMethod;

		public bool IncludeMethod
		{
			get { return m_IncludeMethod; }
			set { m_IncludeMethod = value; }
		}

		protected virtual string FormatException(Exception e)
		{
			if (m_IncludeMethod)
			{
				return String.Concat(MethodName, ": ", e.Message);
			}
			else 
			{
				return e.Message;
			}
		}

		public override void OnException(MethodExecutionArgs eventArgs)
		{
			Exception e = eventArgs.Exception;

			if (e != null)
			{
				Session session = GetSession();
				session.LogException(FormatException(e), e);
			}
		}
	}
}

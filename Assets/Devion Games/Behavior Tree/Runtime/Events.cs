﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DevionGames.BehaviorTrees
{
	public static class Events
	{
		private static Dictionary<string, Delegate> m_GlobalEvents;
		private static Dictionary<object, Dictionary<string, Delegate>> m_Events;

		static Events ()
		{
			Events.m_GlobalEvents = new Dictionary<string, Delegate> ();
			Events.m_Events = new Dictionary<object, Dictionary<string, Delegate>> ();
		}

		public static void Invoke (string eventName)
		{
			Action mDelegate = Events.GetDelegate (eventName) as Action;
			if (mDelegate != null) {
				mDelegate ();
			}
		}

		public static void Invoke (object obj, string eventName)
		{
			Action mDelegate = Events.GetDelegate (obj, eventName) as Action;
			if (mDelegate != null) {
				mDelegate ();
			}
		}

		public static void Invoke<T1> (string eventName, T1 arg1)
		{
			Action<T1> mDelegate = Events.GetDelegate (eventName) as Action<T1>;
			if (mDelegate != null) {
				mDelegate (arg1);
			}
		}

		public static void Invoke<T1> (object obj, string eventName, T1 arg1)
		{
			Action<T1> mDelegate = Events.GetDelegate (obj, eventName) as Action<T1>;
			if (mDelegate != null) {
				mDelegate (arg1);
			}
		}

		public static void Invoke<T1, T2> (string eventName, T1 arg1, T2 arg2)
		{
			Action<T1, T2> mDelegate = Events.GetDelegate (eventName) as Action<T1, T2>;
			if (mDelegate != null) {
				mDelegate (arg1, arg2);
			}
		}

		public static void Invoke<T1,T2> (object obj, string eventName, T1 arg1, T2 arg2)
		{
			Action<T1,T2> mDelegate = Events.GetDelegate (obj, eventName) as Action<T1,T2>;
			if (mDelegate != null) {
				mDelegate (arg1, arg2);
			}
		}

		public static void Invoke<T1, T2, T3> (string eventName, T1 arg1, T2 arg2, T3 arg3)
		{
			Action<T1, T2, T3> mDelegate = Events.GetDelegate (eventName) as Action<T1, T2, T3>;
			if (mDelegate != null) {
				mDelegate (arg1, arg2, arg3);
			}
		}

		public static void Invoke<T1,T2,T3> (object obj, string eventName, T1 arg1, T2 arg2, T3 arg3)
		{
			Action<T1,T2,T3> mDelegate = Events.GetDelegate (obj, eventName) as Action<T1,T2,T3>;
			if (mDelegate != null) {
				mDelegate (arg1, arg2, arg3);
			}
		}

		public static void Register (string eventName, Action handler)
		{
			Events.Register (eventName, (Delegate)handler);
		}

		public static void Register (object obj, string eventName, Action handler)
		{
			Events.Register (obj, eventName, (Delegate)handler);
		}


		public static void Register<T1> (string eventName, Action<T1> handler)
		{
			Events.Register (eventName, (Delegate)handler);
		}

		public static void Register<T1> (object obj, string eventName, Action<T1> handler)
		{
			Events.Register (obj, eventName, (Delegate)handler);
		}

		public static void Register<T1, T2> (string eventName, Action<T1, T2> handler)
		{
			Events.Register (eventName, (Delegate)handler);
		}

		public static void Register<T1, T2> (object obj, string eventName, Action<T1,T2> handler)
		{
			Events.Register (obj, eventName, (Delegate)handler);
		}

		public static void Register<T1, T2, T3> (string eventName, Action<T1, T2, T3> handler)
		{
			Events.Register (eventName, (Delegate)handler);
		}

		public static void Register<T1, T2, T3> (object obj, string eventName, Action<T1, T2,T3> handler)
		{
			Events.Register (obj, eventName, (Delegate)handler);
		}

		public static void Unregister (string eventName, Action handler)
		{
			Events.Unregister (eventName, (Delegate)handler);
		}

		public static void Unregister (object obj, string eventName, Action handler)
		{
			Events.Unregister (obj, eventName, (Delegate)handler);
		}

		public static void Unregister<T1> (string eventName, Action<T1> handler)
		{
			Events.Unregister (eventName, (Delegate)handler);
		}

		public static void Unregister<T1> (object obj, string eventName, Action<T1> handler)
		{
			Events.Unregister (obj, eventName, (Delegate)handler);
		}

		public static void Unregister<T1, T2> (string eventName, Action<T1, T2> handler)
		{
			Events.Unregister (eventName, (Delegate)handler);
		}

		public static void Unregister<T1, T2> (object obj, string eventName, Action<T1, T2> handler)
		{
			Events.Unregister (obj, eventName, (Delegate)handler);
		}

		public static void Unregister<T1, T2, T3> (string eventName, Action<T1, T2, T3> handler)
		{
			Events.Unregister (eventName, (Delegate)handler);
		}

		public static void Unregister<T1, T2, T3> (object obj, string eventName, Action<T1, T2, T3> handler)
		{
			Events.Unregister (obj, eventName, (Delegate)handler);
		}

		private static void Register (string eventName, Delegate handler)
		{
			Delegate mDelegate;
			if (!Events.m_GlobalEvents.TryGetValue (eventName, out mDelegate)) {
				Events.m_GlobalEvents.Add (eventName, handler);
			} else {
				Events.m_GlobalEvents [eventName] = Delegate.Combine (mDelegate, handler);
			}
		}

		private static void Register (object obj, string eventName, Delegate handler)
		{
			if (obj == null)
				return;
			Dictionary<string, Delegate> mEvents;
			Delegate mDelegate;
			if (!Events.m_Events.TryGetValue (obj, out mEvents)) {
				mEvents = new Dictionary<string, Delegate> ();
				Events.m_Events.Add (obj, mEvents);
			}
			if (!mEvents.TryGetValue (eventName, out mDelegate)) {
				mEvents.Add (eventName, handler);
			} else {
				mEvents [eventName] = Delegate.Combine (mDelegate, handler);
			}
		}


		private static void Unregister (string eventName, Delegate handler)
		{
			Delegate mDelegate;
			if (Events.m_GlobalEvents.TryGetValue (eventName, out mDelegate)) {
				Events.m_GlobalEvents [eventName] = Delegate.Remove (mDelegate, handler);
			}
		}

		private static void Unregister (object obj, string eventName, Delegate handler)
		{
			if (obj == null)
				return;
			Dictionary<string, Delegate> mEvents;
			Delegate mDelegate;
			if (Events.m_Events.TryGetValue (obj, out mEvents) && mEvents.TryGetValue (eventName, out mDelegate)) {
				mEvents [eventName] = Delegate.Remove (mDelegate, handler);
			}
		}

		private static Delegate GetDelegate (string eventName)
		{
			Delegate mDelegate;
			if (Events.m_GlobalEvents.TryGetValue (eventName, out mDelegate)) {
				return mDelegate;
			}
			return null;
		}

		private static Delegate GetDelegate (object obj, string eventName)
		{
			Dictionary<string, Delegate> mEvents;
			Delegate mDelegate;
			if (Events.m_Events.TryGetValue (obj, out mEvents) && mEvents.TryGetValue (eventName, out mDelegate)) {
				return mDelegate;
			}
			return null;
		}
	}
}
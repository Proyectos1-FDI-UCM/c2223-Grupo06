using System.Reflection;
using UnityEngine.UI;

public static class UIExtensions
{
	private static readonly MethodInfo toggleMethod;

	static UIExtensions()
	{
		toggleMethod = GetSetMethod(typeof(Toggle));
	}

    	public static void SetSilently(this Toggle toggle, bool value)
	{
		toggleMethod.Invoke(toggle, new object[] { value, false });
	}

	private static MethodInfo GetSetMethod(System.Type type)
	{
		var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
		for (var i = 0; i < methods.Length; i++)
		{
			if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
			{
				return methods[i];
			}
		}

		return null;
	}
}
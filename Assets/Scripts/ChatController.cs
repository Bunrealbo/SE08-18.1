using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private void OnEnable()
	{
		this.TMP_ChatInput.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	private void OnDisable()
	{
		this.TMP_ChatInput.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	private void AddToChatOutput(string newText)
	{
		this.TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		TMP_Text tmp_ChatOutput = this.TMP_ChatOutput;
		if(tmp_ChatOutput != null)
			tmp_ChatOutput.text = string.Concat(new string[]
			{
				tmp_ChatOutput.text,
				"[<#FFFF80>",
				now.Hour.ToString("d2"),
				":",
				now.Minute.ToString("d2"),
				":",
				now.Second.ToString("d2"),
				"</color>] ",
				newText,
				"\n"
			});
		this.TMP_ChatInput.ActivateInputField();
		this.ChatScrollbar.value = 0f;
	}

	public TMP_InputField TMP_ChatInput;

	public TMP_Text TMP_ChatOutput;

	public Scrollbar ChatScrollbar;
}

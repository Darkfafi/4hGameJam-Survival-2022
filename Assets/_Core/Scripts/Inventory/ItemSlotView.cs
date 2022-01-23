﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amountLabel = null;

	[SerializeField]
	private TextMeshProUGUI _buyCostLabel = null;

	[SerializeField]
	private Button _buyButton = null;

	public ItemSlot ItemSlot
	{
		get; private set;
	}

	public void SetItemSlot(ItemSlot itemSlot)
	{
		if(ItemSlot != null)
		{
			ItemSlot.ValueChangedEvent -= OnValueChanged;
		}

		ItemSlot = itemSlot;
		ItemSlot.ValueChangedEvent += OnValueChanged;

		if(itemSlot.BuyCost.HasValue)
		{
			_buyCostLabel.text = itemSlot.BuyCost.Value.ToString();
			_buyButton.gameObject.SetActive(true);
		}
		else
		{
			_buyButton.gameObject.SetActive(false);
		}

		OnValueChanged();
	}

	protected void Awake()
	{
		_buyButton.onClick.AddListener(OnBuyClicked);
	}

	protected void OnDestroy()
	{
		_buyButton.onClick.RemoveListener(OnBuyClicked);

		if (ItemSlot != null)
		{
			ItemSlot.ValueChangedEvent -= OnValueChanged;
		}
	}

	private void OnValueChanged()
	{
		_amountLabel.text = ItemSlot.Amount.ToString();
	}

	private void OnBuyClicked()
	{
		ItemSlot.TryRequestBuy();
	}
}
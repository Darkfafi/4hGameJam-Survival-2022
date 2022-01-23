using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemSlotView : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amountLabel = null;

	[SerializeField]
	private TextMeshProUGUI _buyCostLabel = null;

	[SerializeField]
	private Button _buyButton = null;

	[Header("Audio")]
	[SerializeField]
	private AudioClip _buySound = null;

	public ItemSlot ItemSlot
	{
		get; private set;
	}

	public void SetItemSlot(ItemSlot itemSlot)
	{
		if (ItemSlot != null)
		{
			ItemSlot.ValueChangedEvent -= OnValueChanged;
		}

		ItemSlot = itemSlot;
		ItemSlot.ValueChangedEvent += OnValueChanged;

		if (itemSlot.BuyCost.HasValue)
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
		transform.DOKill(true);
		transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.5f, 5);
	}

	private void OnBuyClicked()
	{
		if (ItemSlot.TryRequestBuy())
		{
			AudioSource.PlayClipAtPoint(_buySound, Camera.main.transform.position);
		}
	}
}
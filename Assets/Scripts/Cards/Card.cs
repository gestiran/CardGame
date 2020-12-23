using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardGame
{
    [RequireComponent(typeof(Image))]
    public class Card : MonoBehaviour, IPointerClickHandler
    {
        public enum CardSuits
        {
            Diamonds,
            Hearts,
            Spades,
            Clubs
        }

        public enum CardTypes
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Quin,
            King,
            Zero
        }

        [HideInInspector] public GameScore gameScore;
        [HideInInspector] public CardsMovemant movemant;
        [HideInInspector] public CardSuits currentSuit;
        [HideInInspector] public CardTypes currentType;
        [HideInInspector] public int cardId;
        [HideInInspector] public bool isClickable;

        [Header("Links")]
        [SerializeField] private Image _suitImage;
        [SerializeField] private Image _cardBack;
        [SerializeField] private Sprite[] _cardSuits;
        [SerializeField] private Text[] _cardText;
        
        [Header("Parameters")]
        [SerializeField] private Color[] _suitColors;

        private bool _cardIsActive;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_cardIsActive && !isClickable) return;
            gameScore.Add();
            movemant.MoveCardToCenter(cardId);
        }
        
        public void Init()
        {
            gameScore = FindObjectOfType<GameScore>();
            movemant = FindObjectOfType<CardsMovemant>();
        }

        public void RecreateCard() => CreateCard((CardSuits) Random.Range(0, 4), (CardTypes) Random.Range(0, 13));

        public void CreateCard(CardSuits cardSuit, CardTypes cardType)
        {
            currentSuit = cardSuit;
            currentType = cardType;
            
            SetSprites();
            SetColor();
            SetText();
        }
        
        public void FlipToFront()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
        public void FlipToBack()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void SetActive()
        {
            isClickable = true;
            _cardIsActive = true;
            SetColor();
        }

        public void SetUnactive()
        {
            isClickable = false;
            _cardIsActive = false;
            SetColor(false);
        }

        #region Private
        
        private void SetSprites() => _suitImage.sprite = _cardSuits[(int) currentSuit];
        
        private void SetColor(bool isActive = true)
        {
            _cardBack.color = (isActive) ? Color.white : Color.gray;
            _suitImage.color = _suitColors[(int) currentSuit] * ((isActive) ? 1f : 0.5f);
            for (int textId = 0; textId < _cardText.Length; textId++) _cardText[textId].color = _suitColors[(int) currentSuit] * ((isActive) ? 1f : 0.5f);
        }

        private void SetText()
        {
            string newCardText = ConvertToText(currentType);
            bool smallScale = false;

            if (newCardText.Length > 1) smallScale = true;

            for (int textId = 0; textId < _cardText.Length; textId++)
            {
                _cardText[textId].text = newCardText;
                if (textId != 1) continue;
                if (smallScale) _cardText[textId].transform.localScale = new Vector3(0.9f, 1.45f, 1f);
                else _cardText[textId].transform.localScale = new Vector3(1.16f, 1.45f, 1f);
            }
        }
        
        private string ConvertToText(CardTypes cardType)
        {
            switch (cardType)
            {
                case CardTypes.Zero: return "A";
                case CardTypes.Two: return "2";
                case CardTypes.Three: return "3";
                case CardTypes.Four: return "4";
                case CardTypes.Five: return "5";
                case CardTypes.Six: return "6";
                case CardTypes.Seven: return "7";
                case CardTypes.Eight: return "8";
                case CardTypes.Nine: return "9";
                case CardTypes.Ten: return "10";
                case CardTypes.Jack: return "J";
                case CardTypes.Quin: return "Q";
                case CardTypes.King: return "K";
            }

            return "";
        }

        #endregion
    }
}
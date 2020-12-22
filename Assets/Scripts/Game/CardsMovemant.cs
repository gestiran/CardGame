using System.Collections;
using CardGame.SaveLoad;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame
{
    public class CardsMovemant : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private GameObject _cardObj;
        [SerializeField] private Transform _hand;
        
        [Header("Parameters")]
        [SerializeField] private int _cardCount;
        [SerializeField] private float _speed;
        [SerializeField] private float _offset;
        [SerializeField] private float _gameCardsOffset;
        
        private Card[] _cards;
        private Card.CardSuits _gameSuit;
        private int _handCardsCount;
        
        private delegate void cardFunc();

        public void NewIteration()
        {
            StopAllCoroutines();
            _gameSuit = GetNewGameSuit();
            ResortCardInHand();
        }
        
        public void StartNewGame()
        {
            StopAllCoroutines();
            FillAllCards();
            StartCoroutine(WaitAllCards(new cardFunc[]{NewIteration}));
            HideAllCards();
            GenerateSuits();
            SortCard();
            MoveCardsToHand();
            ShowGameCards();
        }
        
        public void LoadGame()
        {
            StopAllCoroutines();
            FillAllCards(true);
            _gameSuit = (Card.CardSuits) PlayerPrefs.GetInt("GameSuit");
            StartCoroutine(WaitAllCards(new cardFunc[]{ResortCardInHand}));
            HideAllCards();
            MoveCardsToHand();
            ShowGameCards();
        }

        public void MoveCardToCenter(int cardId)
        {
            StartCoroutine(MoveCard(cardId, gameObject.transform.position));
            StartCoroutine(WaitToCardDestroy(cardId));
        }

        private Card.CardSuits GetNewGameSuit()
        {
            Card.CardSuits result = (Card.CardSuits) Random.Range(0, 4);
            
            for (int i = 0; i < 1000; i++)
            {
                for (int cardId = 0; cardId < _cards.Length; cardId++)
                {
                    if (_cards[cardId].gameObject.activeSelf && _cards[cardId].currentSuit == result) return result;
                }
                
                result = (Card.CardSuits) Random.Range(0, 4);
            }

            return result;
        }
        
        private void FillAllCards(bool isLoad = false)
        {
            _cards = new Card[_cardCount];
                
            for (int cardId = 0; cardId < _cardCount; cardId++)
            {
                GameObject card = Instantiate(_cardObj, transform);
                _cards[cardId] = card.GetComponent<Card>();
                _cards[cardId].cardId = cardId;
                _cards[cardId].Init();
            }

            if (!isLoad && !CardsSaveLoad.HasSaved && !CardsSaveLoad.HasObjectsActives) return;

            bool[] actives = CardsSaveLoad.LoadObjectsActive();
            (int, Card.CardSuits, Card.CardTypes)[] loadedParams = CardsSaveLoad.Load();
            
            for (int cardId = 0; cardId < _cardCount; cardId++)
            {
                _cards[cardId].gameObject.SetActive(actives[cardId]);
                _cards[cardId].cardId = loadedParams[cardId].Item1;
                _cards[cardId].CreateCard(loadedParams[cardId].Item2, loadedParams[cardId].Item3);
                _cards[cardId].CreateCard(_cards[cardId].currentSuit, _cards[cardId].currentType);
            }
        }

        private void HideAllCards()
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++) _cards[cardId].FlipToBack();
        }

        private void GenerateSuits()
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++) _cards[cardId].RecreateCard();
        }
        
        private void ShowAllCards()
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++) _cards[cardId].FlipToFront();
        }
        
        private void SortCard()
        {
            (Card.CardSuits, Card.CardTypes)[] sorted = new (Card.CardSuits, Card.CardTypes)[_cards.Length]; 
            
            int sortedId = 0;
            
            for (int suitIterate = 0; suitIterate < 4; suitIterate++)
            {
                for (int cardIterate = 0; cardIterate < _cards.Length; cardIterate++)
                {
                    if (_cards[cardIterate].currentSuit == (Card.CardSuits) suitIterate)
                    {
                        sorted[sortedId].Item1 = _cards[cardIterate].currentSuit;
                        sorted[sortedId].Item2 = _cards[cardIterate].currentType;
                        sortedId++;
                    }
                }
            }

            for (int cardId = 0; cardId < sorted.Length; cardId++) _cards[cardId].CreateCard(sorted[cardId].Item1,sorted[cardId].Item2);
        }
        
        private void MoveCardsToHand()
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++) StartCoroutine(MoveCard(
                cardId,
                new Vector2(_hand.position.x + (_offset *  cardId) - ((_cards.Length * _offset) / 2), _hand.position.y)));
            ShowAllCards();
        }

        private void ShowGameCards()
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++)
            {
                if (_cards[cardId].currentSuit == _gameSuit) _cards[cardId].SetActive();
                else _cards[cardId].SetUnactive();
            }
        }
        
        private void ResortCardInHand()
        {
            float previousCard = _cards[0].transform.position.x;
            bool isGame = false;
            
            for (int cardId = 0; cardId < _cards.Length; cardId++)
            {
                if (!_cards[cardId].gameObject.activeSelf) continue;

                float currentOffset = _offset;
                
                if (_cards[cardId].currentSuit == _gameSuit)
                {
                    if (!isGame) isGame = true;
                    else currentOffset = _gameCardsOffset;
                }
                else if (isGame)
                {
                    currentOffset = _gameCardsOffset;
                    isGame = false;
                }
                
                Vector2 targetPosition = new Vector2(
                    previousCard + currentOffset,
                    _hand.position.y);

                previousCard = targetPosition.x;
                
                StartCoroutine(MoveCard(cardId, targetPosition));
            }

            ShowGameCards();
        }
        
        private IEnumerator WaitToCardDestroy(int id)
        {
            for (int cardId = 0; cardId < _cards.Length; cardId++) _cards[cardId].isClickable = false;
            yield return new WaitForSecondsRealtime(2f);
            
            _cards[id].gameObject.SetActive(false);
            CardsSaveLoad.Save(_cards);
            CardsSaveLoad.SaveObjectsActive(_cards);
            PlayerPrefs.SetInt("GameSuit", (int)_gameSuit);
            for (int cardId = 0; cardId < _cards.Length; cardId++) _cards[cardId].isClickable = true;
            NewIteration();
        }
        
        private IEnumerator MoveCard(int id, Vector2 newPosition)
        {
            for (int i = 0; i < 10000 && Vector2.Distance(_cards[id].gameObject.transform.position, newPosition) > 0.01f; i++)
            {
                if (_cards[id] == null) yield break;
                _cards[id].transform.position = Vector2.MoveTowards(_cards[id].transform.position,newPosition, _speed);
                yield return new WaitForFixedUpdate();
            }

            _handCardsCount++;
        }

        private IEnumerator WaitAllCards(cardFunc[] funcs)
        {
            for (int i = 0; i < 100; i++)
            {
                if (_handCardsCount >= _cards.Length)
                {
                    for (int funcId = 0; funcId < funcs.Length; funcId++) funcs[funcId]();
                    _handCardsCount = 0;
                    break;
                }
                
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}
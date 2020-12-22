using System.Xml.Linq;
using UnityEngine;

namespace CardGame.SaveLoad
{
    public static class CardsSaveLoad
    {
        private const string _cardBasePref = "CardBase";
        private const string _cardBaseHash = "CardBase_Hash";
        
        private const string _scoreBasePref = "GameScore";
        private const string _scoreBaseHash = "GameScore_Hash";
        
        private const string _objectsActivePref = "ObjectsActive";
        private const string _objectsActiveHash = "ObjectsActive_Hash";

        private const string _root = "base";
        private const string _cardCount = "count";
        private const string _card = "card_";
        private const string _suit = "s";
        private const string _type = "t";
        private const string _id = "id";
        
        #region Cards
        
        public static bool HasSaved =>
            PlayerPrefs.HasKey(_cardBasePref) && XElement.Parse(PlayerPrefs.GetString(_cardBasePref)).GetHashCode() == PlayerPrefs.GetInt(_cardBaseHash);
        
        public static void Save(Card[] cards)
        {
            XElement root = new XElement(_root, new XAttribute(_cardCount, cards.Length));

            for (int cardId = 0; cardId < cards.Length; cardId++)
            {
                root.Add(new XElement(_card + cardId,
                    new XAttribute(_suit, (int)cards[cardId].currentSuit),
                    new XAttribute(_type, (int)cards[cardId].currentType),
                    new XAttribute(_id, cards[cardId].cardId)));
            }
            
            PlayerPrefs.SetString(_cardBasePref, root.ToString());
            PlayerPrefs.SetInt(_cardBaseHash, _root.GetHashCode());
        }
        
        public static (int, Card.CardSuits, Card.CardTypes)[] Load()
        {
            XElement root = XElement.Parse(PlayerPrefs.GetString(_cardBasePref));
            
            (int, Card.CardSuits, Card.CardTypes)[] cards = new (int, Card.CardSuits, Card.CardTypes)[int.Parse(root.Attribute(_cardCount).Value)];

            for (int cardId = 0; cardId < cards.Length; cardId++)
            {
                cards[cardId].Item1 = int.Parse(root.Element(_card + cardId).Attribute(_id).Value);
                cards[cardId].Item2 = (Card.CardSuits)int.Parse(root.Element(_card + cardId).Attribute(_suit).Value);
                cards[cardId].Item3 = (Card.CardTypes)int.Parse(root.Element(_card + cardId).Attribute(_type).Value);
            }
            
            return cards;
        }

        #endregion

        #region Score

        public static bool HasScore => PlayerPrefs.HasKey(_scoreBasePref) && PlayerPrefs.GetInt(_scoreBasePref).GetHashCode() == PlayerPrefs.GetInt(_scoreBaseHash);

        public static int LoadScore() => PlayerPrefs.GetInt(_scoreBasePref, 0);

        public static void SaveScore(int score)
        {
            PlayerPrefs.SetInt(_scoreBasePref, score);
            PlayerPrefs.SetInt(_scoreBaseHash, PlayerPrefs.GetInt(_scoreBasePref).GetHashCode());
        }

        #endregion

        #region Objects

        public static bool HasObjectsActives => PlayerPrefs.HasKey(_objectsActivePref) && PlayerPrefs.GetInt(_objectsActivePref).GetHashCode() == PlayerPrefs.GetInt(_objectsActiveHash);
        
        public static bool[] LoadObjectsActive()
        {
            XElement root = XElement.Parse(PlayerPrefs.GetString(_objectsActivePref));
            
            bool[] result = new bool[int.Parse(root.Attribute(_cardCount).Value)];

            for (int i = 0; i < result.Length; i++) result[i] = root.Element(_card + i).Value == "1";
            
            return result;
        }

        public static void SaveObjectsActive(Card[] objects)
        {
            XElement root = new XElement(_root, new XAttribute(_cardCount, objects.Length));
            
            for (int i = 0; i < objects.Length; i++) root.Add(new XElement(_card + i, (objects[i].gameObject.activeSelf) ? 1 : 0));
            
            PlayerPrefs.SetString(_objectsActivePref, root.ToString());
            PlayerPrefs.SetInt(_objectsActiveHash, _root.GetHashCode());
        } 
        
        #endregion
    }
}
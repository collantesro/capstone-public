namespace CCSInventory.Models
{
    /// <summary>
    /// This class represents the number of Pantry Packs added or removed from inventory. 
    /// </summary>
    public class PantryPackTransaction : TrackedModel
    {
        public int ID { get; set; }
        /// <summary>
        /// Quantity: How many PantryPacks are being added (positive value) or removed (negative value) in this transaction?
        /// </summary>
        /// <value></value>
        public int Qty { get; set; }
        public int PackTypeID { get; set; }
        public string Note { get; set; }
        public PantryPackType PackType { get; set; }
    }
}

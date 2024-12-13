namespace AMDevIT.AI.App.WPF.Models
{
    public class ChatMessage
    {
        #region Properties
        public string? Text 
        { 
            get; 
            set; 
        }

        public bool IsSentByMe 
        { 
            get; 
            set; 
        }

        public string? AvatarColor 
        { 
            get; 
            set; 
        }

        #endregion

        #region Methods

        override public string ToString()
        {
            return $"{DateTimeOffset.UtcNow}] [Me:{IsSentByMe}] {Text}";
        }

        #endregion
    }
}

namespace AMDevIT.AI.App.WPF.Runtime.Messaging
{
    public class ShowTextActionMessage(string text,
                                       string? title = null,
                                       MessageDialogType messageDialogType = MessageDialogType.Information)
    {
        #region Properties

        public string? Title
        {
            get;
            protected set;
        } = title;

        public string Text
        {
            get;
            protected set;
        } = text;

        public MessageDialogType MessageDialogType
        {
            get;
            protected set;
        } = messageDialogType;

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{this.Title}: {this.Text}, {this.MessageDialogType}";
        }

        #endregion
    }
}

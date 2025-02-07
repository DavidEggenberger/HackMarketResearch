window.emojiInit = function emojiInit(elementId, dotNetRef) {

    const pickerOptions =
    {
        data: async () => {
            const response = await fetch(
                'https://cdn.jsdelivr.net/npm/@emoji-mart/data',
            )

            return response.json()
        },
        onEmojiSelect: (emoji) => {
            dotNetRef.invokeMethodAsync('OnEmojiSelected', emoji.native);
        }
    }
    const picker = new EmojiMart.Picker(pickerOptions)

    document.getElementById(elementId).appendChild(picker);
}
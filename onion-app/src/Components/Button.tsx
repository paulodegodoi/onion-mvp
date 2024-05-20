interface IButton {
	text: string // text for button
	type: "submit" | "reset" | "button" | undefined
	bgColor: string
	disabled?: boolean
	onClick?: () => void
}

export function Button({ text, type, bgColor, disabled, onClick }: IButton) {
	if (bgColor == null || bgColor == "") bgColor = "gray"

	const bgClasses = `bg-${bgColor}-500 hover:bg-${bgColor}-700`

	return (
		<button
			className={`${bgClasses} text-white font-bold py-2 px-4 w-44 h-16 rounded my-2 disabled:bg-gray-300 disabled:text-gray-600`}
			type={type}
			disabled={disabled != null ? disabled : false}
			onClick={onClick}
		>
			{text}
		</button>
	)
}

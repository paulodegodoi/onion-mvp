import logo from "../assets/motion-blur.svg"

interface ILoading {
	message?: string
}
export function Loading({ message }: ILoading) {
	if (message == null || message == "") {
		message = "Carregando dados..."
	}
	return (
		<div className="w-screen h-screen bg-gray-500 bg-opacity-50 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
			<div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
				<img src={logo} width={100} className="m-auto z-50" />
				<span className="font-bold text-blue-900 text-xl">{message}</span>
			</div>
		</div>
	)
}

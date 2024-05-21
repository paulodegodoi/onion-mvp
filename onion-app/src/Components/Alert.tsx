import { IoMdCloseCircle } from "react-icons/io"

interface IAlert {
	message: string
	setIsShowAlert: (bol: boolean) => void
}

export function Alert({ message, setIsShowAlert }: IAlert) {
	return (
		<div className="w-screen h-screen bg-gray-500 bg-opacity-50 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
			<div className="bg-white border-blue-900 border absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 p-10 w-11/12 sm:w-4/6 md:w-6/12 rounded text-center">
				<span className="font-bold text-blue-900 text-xl">{message}</span>
				<button className="absolute top-0 right-0" onClick={() => setIsShowAlert(false)}>
					<IoMdCloseCircle size={22} />
				</button>
			</div>
		</div>
	)
}

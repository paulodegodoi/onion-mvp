import { ChangeEvent, FormEvent, useContext, useRef, useState } from "react"
import { Button } from "../Components/Button"
import logo from "../assets/motion-blur.svg"
import { OrdersDataContext } from "../contexts/OrdersDataContext"
import { Link, useNavigate } from "react-router-dom"

export function Home() {
	const [selectedFile, setSelectedFile] = useState<File | null>(null)
	const [isLoadingData, setIsLoadingData] = useState(false)
	const fileInputRef = useRef<HTMLInputElement | null>(null)

	const navigate = useNavigate()
	// ordersData context
	const { ordersData, setOrdersData } = useContext(OrdersDataContext)

	/** Disparada sempre que o arquivo do input é alterado para salvar em selectedFile */
	function handleFileChange(event: ChangeEvent<HTMLInputElement>) {
		if (event.target.files && event.target.files.length > 0) {
			setSelectedFile(event.target.files[0])
		}
	}

	/** Faz o post na api com o arquivo selecionado e retorna os dados salvando-os em ordersData */
	async function handleSubmit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()

		if (!selectedFile) {
			alert("Adicione uma planilha antes para carregar os dados.")
			return
		}

		const formData = new FormData()
		formData.append("file", selectedFile)

		setIsLoadingData(true)
		try {
			const response = await fetch(
				"http://192.168.0.67:5111/api/Onion/builddetailsfororder",
				{
					method: "POST",
					body: formData,
				}
			)

			if (response.ok) {
				var json = await response.json()
				setOrdersData(json)
				navigate("dados-informativos")
			} else {
				alert("Falha ao fazer o upload.")
			}
		} catch (error) {
			console.error("Falha ao fazer o upload do arquivo: ", error)
			alert("Falha ao fazer o upload do arquivo")
		}
		setIsLoadingData(false)
	}

	/** Dispara o click no input de arquivo */
	const handleButtonClick = () => {
		if (fileInputRef.current) {
			fileInputRef.current.click()
		}
	}

	return (
		<>
			{isLoadingData && (
				<div className="w-screen h-screen bg-gray-500 bg-opacity-50 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
					<div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
						<img src={logo} width={100} className="z-50" />
						<span className="italic text-blue-700 text-xl">Carregando dados...</span>
					</div>
				</div>
			)}
			<div className="text-center items-center">
				<div className="m-3">
					<p className="text-2xl">Bem-Vindo ao</p>
					<h1 className="text-4xl">Onion ChartApp</h1>
				</div>
				<div className="my-10">
					<p className="text-xl mb-2">
						Siga os seguintes passos para gerar os gráficos informativos:
					</p>
					<p className=" text-xl italic">
						1 - Faça o download e preencha nossa planilha modelo
					</p>
					<p className="text-xl italic">2 - Adicione a planilha preenchida</p>
					<p className="text-xl italic">3 - Clique em gerar dados e pronto!</p>
				</div>
				<a
					href="http://192.168.0.67:5111/api/Onion/planilha-modelo"
					className="block mb-3 w-44 mx-auto"
				>
					<Button
						text="Baixar planilha modelo"
						bgColor="gray"
						type="button"
						disabled={isLoadingData}
					/>
				</a>
				<form onSubmit={handleSubmit}>
					<input
						type="file"
						id="file-upload"
						onChange={handleFileChange}
						ref={fileInputRef}
						className="hidden"
					/>
					<div className="flex flex-col justify-center items-center sm:flex-row gap-2">
						<Button
							text={
								selectedFile == null
									? "Adicionar arquivo"
									: `Substituir arquivo ${selectedFile!.name}`
							}
							bgColor="blue"
							type="button"
							onClick={handleButtonClick}
							disabled={isLoadingData}
						/>
						{ordersData == null || selectedFile != null ? (
							<Button
								text="Gerar dados"
								bgColor="green"
								type="submit"
								disabled={isLoadingData || !selectedFile}
							/>
						) : (
							<Link to="/dados-informativos">
								<Button text="Retomar dados" bgColor="yellow" type="button" />
							</Link>
						)}
					</div>
				</form>
			</div>
		</>
	)
}

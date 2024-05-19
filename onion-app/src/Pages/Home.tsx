import { ChangeEvent, FormEvent, useRef, useState } from "react"
import { Pedido } from "../types/pedido"
import { Chart } from "../Components/Chart"
import { ChartType } from "../enums/chartType"
import { OrdersList } from "../Components/OrdersList"
import { Button } from "../Components/Button"
import logo from "../assets/motion-blur.svg"

export function Home() {
	const [selectedFile, setSelectedFile] = useState<File | null>(null)
	const [ordersData, setOrdersData] = useState<Pedido[] | null>(null)
	const [isLoadingData, setIsLoadingData] = useState(false)
	const fileInputRef = useRef<HTMLInputElement | null>(null)

	function handleFileChange(event: ChangeEvent<HTMLInputElement>) {
		if (event.target.files && event.target.files.length > 0) {
			setSelectedFile(event.target.files[0])
		}
	}

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
			const response = await fetch("http://localhost:5111/api/Onion/builddetailsfororder", {
				method: "POST",
				body: formData,
			})

			if (response.ok) {
				var json = await response.json()
				console.log(json)
				setOrdersData(json)
			} else {
				alert("File upload failed")
				// Handle failure response here
			}
		} catch (error) {
			console.error("Error uploading file:", error)
			alert("Error uploading file")
		}
		setIsLoadingData(false)
	}

	const handleButtonClick = () => {
		if (fileInputRef.current) {
			fileInputRef.current.click()
		}
	}

	return (
		<main className="">
			{isLoadingData && (
				<div className="w-screen h-screen bg-gray-500 bg-opacity-20 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
					<div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
						<img src={logo} width={100} />
						<span className="italic text-blue-700">Carregando dados...</span>
					</div>
				</div>
			)}
			{ordersData == null ? (
				<div className="text-center">
					<div className="m-3">
						<p className="text-2xl">BEM-VINDO</p>
						<p className="bold">ao</p>
						<h1 className="text-3xl">ChartApp</h1>
					</div>
					<div className="my-10">
						<p>Siga os seguintes passos para gerar os gráficos informativos:</p>
						<p className=" italic">
							1 - Faça o download e preencha nossa planilha modelo
						</p>
						<p className="italic">2 - Adicione a planilha preenchida</p>
						<p className="italic">3 - Aguarde os dados serem exibidos</p>
					</div>
					<a
						href="http://localhost:5111/api/Onion/planilha-modelo"
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
							{/* <p className="">{selectedFile?.name}</p> */}

							<Button
								text="Gerar dados"
								bgColor="green"
								type="submit"
								disabled={isLoadingData}
							/>
						</div>
					</form>
				</div>
			) : (
				<div className="flex flex-col items-center py-4">
					{/* <Link to="/">Gerar novos dados</Link> */}
					<Button
						bgColor="yellow"
						text="Gerar novos dados"
						type="button"
						onClick={() => setOrdersData(null)}
					/>
					<div className="w-screen flex flex-col items-center lg:flex-row md:justify-around">
						<Chart ordersData={ordersData} chartType={ChartType.Regiao} />
						<Chart ordersData={ordersData} chartType={ChartType.Produto} />
					</div>
					<OrdersList ordersData={ordersData} />
				</div>
			)}
		</main>
	)
}

import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { Produto } from "../types/produto"
import { ProdutoForm } from "../Components/ProdutoForm"
import { Alert } from "../Components/Alert"
import { Loading } from "../Components/Loading"

export function EditProduct() {
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const [isShowAlert, setIsShowAlert] = useState(false)
	const [isLoadingData, setIsLoadingData] = useState(false)
	const [message, setMessage] = useState("")
	const [product, setProduct] = useState<Produto | null>(null)
	const [productName, setProductName] = useState("")
	const [productValue, setProductValue] = useState<number>(0)

	// Obter o produto pelo id
	useEffect(() => {
		const fetchProduct = async () => {
			try {
				const response = await fetch(`http://localhost:5111/api/produtos/${id}`)
				if (response.ok) {
					const json = await response.json()
					setProduct(json)
					setProductName(json.nome)
					setProductValue(json.valor)
				} else {
					console.error("Falha ao obter o produto")
				}
			} catch (error) {
				console.error("Erro:", error)
			}
		}

		fetchProduct()
	}, [id])

	// Atualiza o produto
	const handleSubmitUpdate = async (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault()

		const updatedProduct: Produto = {
			id: Number(id),
			nome: productName,
			valor: productValue,
		}

		// validação nos inputs por enquanto
		if (productName == "" || productValue <= 0) {
			return
		}

		try {
			const response = await fetch(`http://localhost:5111/api/produtos/${id}`, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(updatedProduct),
			})

			if (response.ok) {
				setIsShowAlert(true)
				setMessage("Produto atualizado com sucesso!")

				// Aguardar 3 segundos antes de navegar para /produtos
				setTimeout(() => {
					navigate("/produtos")
				}, 3000)
			} else {
				setIsLoadingData(false)
				let errorText = await response.text()
				if (errorText == "Failed to fetch")
					errorText = "Falha na comunicação com o servidor"
				throw new Error(errorText)
			}
		} catch (error) {
			// mostra a mensagem de erro
			setIsShowAlert(true)
			setMessage((error as Error).message)
		}
	}

	return (
		<>
			{isShowAlert && <Alert message={message} setIsShowAlert={setIsShowAlert} />}
			{isLoadingData && <Loading message="Atualizando produto..." />}
			<div className="mt-3 flex flex-col items-center">
				<h1 className="text-center text-2xl">Editar Produto</h1>

				<ProdutoForm
					onSubmit={handleSubmitUpdate}
					productName={productName}
					setProductName={setProductName}
					productValue={productValue}
					setProductValue={setProductValue}
				/>
			</div>
		</>
	)
}

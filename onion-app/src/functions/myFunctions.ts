import { ChartType } from "../enums/chartType"
import { ApexSeriesOptions } from "../types/apexSeriesOptions"
import { Pedido } from "../types/pedido"
import { ProdutoTypes } from "../types/produto"

/** Retorna a região pelo estado (uf) informado */
export function getRegionByUF(uf: string): Regiao | "Sigla de estado inválida" {
	const siglaUpperCase = uf.toUpperCase() as SiglaEstado
	if (siglaUpperCase in estados) {
		return estados[siglaUpperCase]
	}
	return "Sigla de estado inválida"
}

/** Retorna o index da lista de produtos pelo nome */
export function getProdutoIndexByName(productName: string): ProdutoTypes | "Produto inválido" {
	if (productName == "Televisão") productName = "Televisao"

	const productType = productName.toLocaleLowerCase() as ProdutoTypes
	if (productType in produtos) {
		return produtos[productType]
	}
	return "Produto inválido"
}

/** Retorna as configurações para utiliza-las no componente Chart (ReactApexCharts) */
export function getApexOptionsByChartType(ordersData: Pedido[], chartType: ChartType) {
	// series para configurar ApexSeriesOptions
	let series: number[] = []

	// lógica para tipo região
	if (chartType == ChartType.Regiao) {
		// regionCount para usar em series
		const regionCount: Record<Regiao, number> = {
			Norte: 0,
			Nordeste: 0,
			CentroOeste: 0,
			Sudeste: 0,
			Sul: 0,
		}
		ordersData.forEach((order) => {
			const region = getRegionByUF(order.uf)
			if (region !== "Sigla de estado inválida") {
				regionCount[region]++
			}
		})
		series = [
			regionCount.Norte,
			regionCount.Nordeste,
			regionCount.CentroOeste,
			regionCount.Sudeste,
			regionCount.Sul,
		]
	}
	// lógica para tipo produto
	else if (chartType == ChartType.Produto) {
		const produtoCount: Record<ProdutoTypes, number> = {
			celular: 0,
			notebook: 0,
			televisao: 0,
		}
		ordersData.forEach((order) => {
			const productName = getProdutoIndexByName(order.produto.nome)
			if (productName != "Produto inválido") produtoCount[productName]++
		})

		series = [produtoCount.celular, produtoCount.notebook, produtoCount.televisao]
	}

	// objeto de configurações do Chart
	let apexSeriesOptions: ApexSeriesOptions = {
		series: [],
		options: {},
	}

	switch (chartType) {
		case ChartType.Regiao:
			apexSeriesOptions.options = {
				labels: ["Norte", "Nordeste", "Centro-Oeste", "Sudeste", "Sul"],
			}
			break
		case ChartType.Produto:
			apexSeriesOptions.options = {
				labels: ["Celular", "Notebook", "Televisão"],
			}
			break
	}

	apexSeriesOptions.options.responsive = responsive
	// apexSeriesOptions.options.chart = { width: 600 }
	apexSeriesOptions.options.legend = legend
	apexSeriesOptions.series = series
	return apexSeriesOptions
}

/** Formatar data string para data formato brasileiro */
export function formatToBrazilianDate(dateString: string): string {
	const date = new Date(dateString)
	const options: Intl.DateTimeFormatOptions = {
		day: "2-digit",
		month: "2-digit",
		year: "numeric",
	}
	const brazilianDateFormatter = new Intl.DateTimeFormat("pt-BR", options)

	return brazilianDateFormatter.format(date)
}

/** formatar moeda R$ */
export function formatToCurrency(number: number): string {
	const currencyFormatter = new Intl.NumberFormat("pt-BR", {
		style: "currency",
		currency: "BRL", // Brazilian Real
		minimumFractionDigits: 2, // Ensure two decimal places
	})

	return currencyFormatter.format(number)
}

// estados e suas regiões
const estados: Record<SiglaEstado, Regiao> = {
	AC: "Norte",
	AL: "Nordeste",
	AP: "Norte",
	AM: "Norte",
	BA: "Nordeste",
	CE: "Nordeste",
	DF: "CentroOeste",
	ES: "Sudeste",
	GO: "CentroOeste",
	MA: "Nordeste",
	MT: "CentroOeste",
	MS: "CentroOeste",
	MG: "Sudeste",
	PA: "Norte",
	PB: "Nordeste",
	PR: "Sul",
	PE: "Nordeste",
	PI: "Nordeste",
	RJ: "Sudeste",
	RN: "Nordeste",
	RS: "Sul",
	RO: "Norte",
	RR: "Norte",
	SC: "Sul",
	SP: "Sudeste",
	SE: "Nordeste",
	TO: "Norte",
}

// produtos
const produtos: Record<string, ProdutoTypes> = {
	celular: "celular",
	notebook: "notebook",
	televisao: "televisao",
}

// configuração de responsividade Chart
const responsive = [
	{
		// breakpoint: 768,
		// options: {
		// 	chart: {
		// 		width: 350, // Full width for mobile devices
		// 	},
		// },
	},
]

// legenda do Chart
const legend: ApexLegend = {
	show: true,
	showForSingleSeries: false,
	showForNullSeries: true,
	showForZeroSeries: true,
	position: "bottom",
	horizontalAlign: "center",
	floating: false,
	fontSize: "14px",
	fontFamily: "Roboto, sans-serif",
	fontWeight: 400,
	formatter: undefined,
	inverseOrder: false,
	width: undefined,
	height: undefined,
	tooltipHoverFormatter: undefined,
	customLegendItems: [],
	offsetX: 0,
	offsetY: 0,
	labels: {
		colors: undefined,
		useSeriesColors: false,
	},
}

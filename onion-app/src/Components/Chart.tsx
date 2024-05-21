import ReactApexCharts from "react-apexcharts"
import { Pedido } from "../types/pedido"
import { getApexOptionsByChartType } from "../functions/myFunctions"
import { ChartType } from "../enums/chartType"

interface ChartInterface {
	ordersData: Pedido[]
	chartType: ChartType
}

export function Chart(chartObject: ChartInterface) {
	let apexSeriesOptions = getApexOptionsByChartType(chartObject.ordersData, chartObject.chartType)

	let chartTitle = ""
	if (chartObject.chartType == ChartType.Regiao) chartTitle = "Região"
	else if (chartObject.chartType == ChartType.Produto) chartTitle = "Produto"

	return (
		<div className="my-4 w-screen md:max-w-md">
			{chartTitle != "" && (
				<h3 className="text-center">
					Vendas por <span className="font-bold text-lg">{chartTitle}</span>
				</h3>
			)}

			<ReactApexCharts
				width="100%"
				options={apexSeriesOptions.options}
				series={apexSeriesOptions.series}
				// height={380}
				type="pie" // gráfico pizza
			/>
		</div>
	)
}

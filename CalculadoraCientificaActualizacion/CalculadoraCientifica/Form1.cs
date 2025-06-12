using System;
using System.Data;
using System.Windows.Forms;

namespace CalculadoraCientifica
{
    public partial class Form1 : Form
    {
        // Variables globales
        double primero = 0, segundo = 0, resultado = 0;
        string operador = string.Empty;
        bool operacionRepetida = false;
        double memoria = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void ValidarEntrada()
        {
            if (string.IsNullOrEmpty(tbx_screen.Text))
            {
                tbx_screen.Text = "Error: Ingresa un valor";
                throw new InvalidOperationException("Se esperaba un valor numérico en el campo de entrada.");
            }

            if (!double.TryParse(tbx_screen.Text, out _))
            {
                tbx_screen.Text = "Error: Valor no válido";
                throw new FormatException("El valor en el campo de entrada no es un número válido.");
            }
        }

        private void LimpiarSiHayError()
        {
            if (tbx_screen.Text.StartsWith("Error"))
            {
                tbx_screen.Clear();
            }
        }

        // Métodos para los botones de números
        private void btn_0_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "0";
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "1";
        }

        private void btn_2_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "2";
        }

        private void btn_3_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "3";
        }

        private void btn_4_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "4";
        }

        private void btn_5_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "5";
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "6";
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "7";
        }

        private void btn_8_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "8";
        }

        private void btn_9_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += "9";
        }

        private void btn_punto_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            if (!tbx_screen.Text.Contains("."))
            {
                tbx_screen.Text += ".";
            }
        }

        // Métodos para los botones de operaciones
        private void btn_suma_Click(object sender, EventArgs e)
        {
            AgregarOperacion("+");
        }

        private void btn_resta_Click(object sender, EventArgs e)
        {
            AgregarOperacion("-");
        }

        private void btn_multi_Click(object sender, EventArgs e)
        {
            AgregarOperacion("*");
        }

        private void btn_div_Click(object sender, EventArgs e)
        {
            AgregarOperacion("/");
        }

        private void AgregarOperacion(string operadorSeleccionado)
        {
            LimpiarSiHayError();

            if (!string.IsNullOrEmpty(tbx_screen.Text))
            {
                if (operacionRepetida)
                {
                    tbx_screen2.Text = tbx_screen.Text + " " + operadorSeleccionado + " "; // Reiniciar tbx_screen2 con solo el resultado y la nueva operación
                    operacionRepetida = false; // Resetear la bandera de operación repetida
                }
                else
                {
                    tbx_screen2.Text += tbx_screen.Text + " " + operadorSeleccionado + " "; // Añadir el resultado anterior a tbx_screen2
                }

                tbx_screen.Clear();
                operador = operadorSeleccionado;
            }
            else if (tbx_screen2.Text.EndsWith(" ") && !string.IsNullOrEmpty(operadorSeleccionado))
            {
                tbx_screen2.Text = tbx_screen2.Text.TrimEnd() + " " + operadorSeleccionado + " ";
                operador = operadorSeleccionado;
            }
        }


        private void btn_igual_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si se trata de una operación especial (%, xy, yvx)
                if (operador == "%" || operador == "xy" || operador == "yvx")
                {
                    if (!operacionRepetida)
                    {
                        segundo = double.Parse(tbx_screen.Text);
                        operacionRepetida = true;
                    }
                    else
                    {
                        primero = resultado; // Usar el resultado anterior como el primer operando para operaciones repetidas
                    }

                    switch (operador)
                    {
                        case "%":
                            resultado = (primero * segundo) / 100;
                            tbx_screen2.Text = $"{primero} % {segundo} =";
                            break;
                        case "xy":
                            resultado = Math.Pow(primero, segundo);
                            tbx_screen2.Text = $"{primero} ^ {segundo} =";
                            break;
                        case "yvx":
                            resultado = Math.Pow(segundo, 1.0 / primero);
                            tbx_screen2.Text = $"{primero}√{segundo} =";
                            break;
                        default:
                            tbx_screen.Text = "Error";
                            return;
                    }

                    tbx_screen.Text = resultado.ToString("G15");
                    operador = string.Empty; // Limpiar el operador después de la operación
                }
                else
                {
                    // Para operaciones aritméticas estándar (+, -, *, /)
                    if (!operacionRepetida)
                    {
                        segundo = double.Parse(tbx_screen.Text); // Capturamos el segundo número
                        string expresionCompleta = tbx_screen2.Text + tbx_screen.Text;

                        DataTable dt = new DataTable();
                        var resultadoCompleto = dt.Compute(expresionCompleta, "");

                        tbx_screen.Text = Convert.ToDouble(resultadoCompleto).ToString("G15");
                        tbx_screen2.Text = expresionCompleta + " = "; // Muestra la operación completa y el resultado en tbx_screen2

                        primero = Convert.ToDouble(resultadoCompleto);
                        resultado = primero; // Guardamos el resultado como el primer número para la próxima repetición
                        operacionRepetida = true;
                    }
                    else
                    {
                        // Para operaciones repetidas, usar el último operador y segundo valor
                        string expresionCompleta = $"{resultado} {operador} {segundo}";

                        DataTable dt = new DataTable();
                        var resultadoCompleto = dt.Compute(expresionCompleta, "");

                        tbx_screen.Text = Convert.ToDouble(resultadoCompleto).ToString("G15");
                        tbx_screen2.Text = expresionCompleta + " = ";

                        resultado = Convert.ToDouble(resultadoCompleto);
                        primero = resultado; // Actualiza el primer operando para la siguiente repetición
                    }
                }
            }
            catch (Exception)
            {
                tbx_screen.Text = "Error";
            }
        }



        private void btn_pi_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Text += Math.PI.ToString("G15");
            tbx_screen2.Text += $"";
        }

        private void btn_borrarTodo_Click(object sender, EventArgs e)
        {
            tbx_screen.Clear();
            tbx_screen2.Clear();
            operacionRepetida = false;
            primero = 0;
            segundo = 0;
            resultado = 0;
            operador = string.Empty;
        }

        private void btn_borrarUno_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            if (tbx_screen.Text.Length > 1)
            {
                tbx_screen.Text = tbx_screen.Text.Substring(0, tbx_screen.Text.Length - 1);
            }
            else
            {
                tbx_screen.Text = "";
            }
        }

        private void btn_c_Click(object sender, EventArgs e)
        {
            LimpiarSiHayError();
            tbx_screen.Clear();
        }

        // Métodos para operaciones avanzadas
        private void btn_x_y_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                primero = double.Parse(tbx_screen.Text);
                operador = "xy";

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"{tbx_screen.Text} ^ ";
                }
                else
                {
                    tbx_screen2.Text += $"{primero} ^ ";
                }

                tbx_screen.Clear();
                operacionRepetida = false;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_yRaizX_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                primero = double.Parse(tbx_screen.Text);
                operador = "yvx";

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"{tbx_screen.Text}√";
                }
                else
                {
                    tbx_screen2.Text += $"{primero}√";
                }

                tbx_screen.Clear();
                operacionRepetida = false;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_porcentaje_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                operador = "%";

                // Verificar si es una operación repetida
                if (operacionRepetida)
                {
                    tbx_screen2.Text = tbx_screen.Text + " % ";  // Mostrar solo la nueva operación en tbx_screen2
                }
                else
                {
                    tbx_screen2.Text += tbx_screen.Text + " % ";  // Mostrar la operación completa en tbx_screen2
                }

                primero = double.Parse(tbx_screen.Text);
                tbx_screen.Clear();
                operacionRepetida = false;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }


        // Funciones científicas
        private void btn_sin_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoSin = Math.Sin(valor * Math.PI / 180); // Calcula el seno en grados

                // Evaluar si estamos en medio de una operación aritmética
                if (!string.IsNullOrEmpty(tbx_screen2.Text) && !operacionRepetida)
                {
                    tbx_screen.Text = resultadoSin.ToString("G15");
                    tbx_screen2.Text += $"sin({valor})"; // Añade la operación trigonométrica a la expresión
                }
                else
                {
                    // Si no hay operación pendiente, simplemente muestra el resultado
                    tbx_screen2.Text += $"sin({valor})";
                    tbx_screen.Text = resultadoSin.ToString("G15");
                }

                primero = resultadoSin;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_cos_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoCos = Math.Cos(valor * Math.PI / 180); // Calcula el coseno en grados

                // Evaluar si estamos en medio de una operación aritmética
                if (!string.IsNullOrEmpty(tbx_screen2.Text) && !operacionRepetida)
                {
                    tbx_screen.Text = resultadoCos.ToString("G15");
                    tbx_screen2.Text += $"cos({valor})"; // Añade la operación trigonométrica a la expresión
                }
                else
                {
                    // Si no hay operación pendiente, simplemente muestra el resultado
                    tbx_screen2.Text += $"cos({valor})";
                    tbx_screen.Text = resultadoCos.ToString("G15");
                }

                primero = resultadoCos;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }


        private void btn_tan_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoTan = Math.Tan(valor * Math.PI / 180); // Calcula la tangente en grados

                // Evaluar si estamos en medio de una operación aritmética
                if (!string.IsNullOrEmpty(tbx_screen2.Text) && !operacionRepetida)
                {
                    tbx_screen.Text = resultadoTan.ToString("G15");
                    tbx_screen2.Text += $"tan({valor})"; // Añade la operación trigonométrica a la expresión
                }
                else
                {
                    // Si no hay operación pendiente, simplemente muestra el resultado
                    tbx_screen2.Text += $"tan({valor})";
                    tbx_screen.Text = resultadoTan.ToString("G15");
                }

                primero = resultadoTan;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_x2_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoX2 = Math.Pow(valor, 2); // Eleva al cuadrado

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"{tbx_screen.Text}^2";
                }
                else
                {
                    tbx_screen2.Text += $"{valor}^2";
                }

                tbx_screen.Text = resultadoX2.ToString("G15");

                primero = resultadoX2;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_x3_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoX3 = Math.Pow(valor, 3); // Eleva al cubo

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"{tbx_screen.Text}^3";
                }
                else
                {
                    tbx_screen2.Text += $"{valor}^3";
                }

                tbx_screen.Text = resultadoX3.ToString("G15");

                primero = resultadoX3;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_raizCuadrada_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoRaiz = Math.Sqrt(valor);

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"√({tbx_screen.Text})";
                }
                else
                {
                    tbx_screen2.Text += $"√({valor})";
                }

                tbx_screen.Text = resultadoRaiz.ToString("G15");

                primero = resultadoRaiz;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_log_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoLog = Math.Log10(valor);

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"log({tbx_screen.Text})";
                }
                else
                {
                    tbx_screen2.Text += $"log({valor})";
                }

                tbx_screen.Text = resultadoLog.ToString("G15");

                primero = resultadoLog;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_exp_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoExp = Math.Exp(valor); // Calcula la exponencial

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"exp({tbx_screen.Text})";
                }
                else
                {
                    tbx_screen2.Text += $"exp({valor})";
                }

                tbx_screen.Text = resultadoExp.ToString("G15");

                primero = resultadoExp;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_cambiarSigno_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                tbx_screen.Text = (-valor).ToString(); // Cambia el signo del número
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }


        private void btn_diezElevadoX_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoDiezElevadoX = Math.Pow(10, valor); // Calcula 10^x

                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"10^({tbx_screen.Text})";
                }
                else
                {
                    tbx_screen2.Text += $"10^({valor})";
                }

                tbx_screen.Text = resultadoDiezElevadoX.ToString("G15");

                primero = resultadoDiezElevadoX;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_MasMemoria_Click(object sender, EventArgs e)
        {
            try
            {
                double valorActual = double.Parse(tbx_screen.Text);
                memoria += valorActual;
                tbx_screen2.Text = "Memoria + " + valorActual.ToString();
            }
            catch (Exception)
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_MenosMemoria_Click(object sender, EventArgs e)
        {
            try
            {
                double valorActual = double.Parse(tbx_screen.Text);
                memoria -= valorActual;
                tbx_screen2.Text = "Memoria - " + valorActual.ToString();
            }
            catch (Exception)
            {
                tbx_screen.Text = "Error";
            }
        }

        private void btn_RecuperarMemoria_Click(object sender, EventArgs e)
        {
            tbx_screen.Text = memoria.ToString("G15");
            tbx_screen2.Text = "Recuperar Memoria";
        }

        private void btn_BorrarMemoria_Click(object sender, EventArgs e)
        {
            memoria = 0;
            tbx_screen2.Text = "Memoria Borrada";
        }


        private void btn_tresRaizCuadrada_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarEntrada();
                double valor = double.Parse(tbx_screen.Text);
                double resultadoRaizCubica = Math.Pow(valor, 1.0 / 3.0); // Calcula la raíz cúbica

                // Verificar si es una operación repetida
                if (operacionRepetida)
                {
                    tbx_screen2.Text = $"3√({tbx_screen.Text})";  // Mostrar solo la nueva operación en tbx_screen2
                }
                else
                {
                    tbx_screen2.Text += $"3√({valor})"; // Mostrar la operación completa en tbx_screen2
                }

                tbx_screen.Text = resultadoRaizCubica.ToString("G15");

                primero = resultadoRaizCubica;
                operacionRepetida = true;
                operador = string.Empty;
            }
            catch
            {
                tbx_screen.Text = "Error";
            }
        }
    }
}

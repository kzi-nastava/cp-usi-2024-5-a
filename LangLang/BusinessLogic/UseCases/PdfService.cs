﻿using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System.IO;
using System;
using System.Collections.Generic;
using LangLang.Domain.Models;

namespace LangLang.BusinessLogic.UseCases
{
    public class PdfService
    {
        public static PdfDocument GeneratePdf<T>(T data, string[] headers, string reportName, Func<T, PdfGrid> DataToGrid)
        {
            PdfDocument document = new PdfDocument();
                
            document.PageSettings.Size = PdfPageSize.A4;

            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString(reportName, font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

            PdfGrid pdfGrid = DataToGrid(data);
            pdfGrid.Headers.Add(1);

            PdfGridRow pdfGridHeader = pdfGrid.Headers[0];
            for (int i = 0; i < headers.Length; i++)
                pdfGridHeader.Cells[i].Value = headers[i];

            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, 40));

            string fileName = $"{reportName}.pdf";
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
                document.Save(stream);

            return document;
        }

        public PdfGrid DataToGrid(Dictionary<string, double> data)
        {
            PdfGrid grid = new PdfGrid();
            grid.Columns.Add(2);
            foreach (var item in data)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = item.Key;
                row.Cells[1].Value = item.Value.ToString();
            }
            return grid;
        }

        public PdfGrid DataToGrid(Dictionary<(Course, int), double> data)
        {
            PdfGrid grid = new();
            grid.Columns.Add(4);
            foreach (var item in data)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = item.Key.Item1.Language;
                row.Cells[1].Value = item.Key.Item1.Level.ToString();
                row.Cells[2].Value = item.Key.Item2.ToString();
                row.Cells[3].Value = item.Value.ToString();
            }

            return grid;
        }

        public PdfGrid DataToGrid(Dictionary<Course, int> data)
        {
            PdfGrid grid = new();
            grid.Columns.Add(2);
            foreach (var item in data)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = item.Key.Language + " " + item.Key.Level.ToString();
                row.Cells[1].Value = item.Value.ToString();
            }

            return grid;
        }
    }
}

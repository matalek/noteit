using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Diagnostics;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NoteIt
{
    class VerticalPrintStrategy : PrintStrategy
    {
        protected override PrintStrategy.OrientationType Orientation { get { return OrientationType.Vertical; } }

        public override void Print(Note note, FileStream fs, bool withSlideNumbers)
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4, Margins[0], Margins[1], Margins[2], Margins[3]);
            PdfWriter wri = PdfWriter.GetInstance(doc, fs);

            TableHeader tevent = new TableHeader();
            wri.PageEvent = tevent;
            tevent.Header = note.Title;
            tevent.Orientation = OrientationType.Vertical;

            doc.Open();

            PrintTitle(note, doc);

            PdfPTable table;
            if (withSlideNumbers)
            {
                table = new PdfPTable(3);
                table.SetWidths(new int[] { 1, 14, 10 });
            }
            else
            {
                table = new PdfPTable(2);
                table.SetWidths(new int[] { 14, 10 });
            }

            table.WidthPercentage = 92;

            foreach (Slide slide in note.SlidesList)
                PrintSlide(slide, table, withSlideNumbers);                

            doc.Add(table);
            doc.Close();
        }

        private void PrintSlide(Slide slide, PdfPTable table, bool withSlideNumbers)
        {
            PdfPCell cell;
            Font font = FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10);

            if (withSlideNumbers)
            {
                int nr = slide.Nr + 1;
                cell = new PdfPCell(new iTextSharp.text.Paragraph(nr.ToString(), font));
                cell.BorderWidth = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);  
            }

            if (slide.Image != null)
            {
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(slide.Image, System.Drawing.Imaging.ImageFormat.Bmp);
                pdfImage.ScaleToFit(250, 250);
                cell = new PdfPCell(pdfImage);
            }
            else
                // PDF image is not set for this slide (PDF slides weren't imported or user decided to add additional slides)
                cell = new PdfPCell();

            cell.BorderWidth = 0;
            table.AddCell(cell);

            
            cell = new PdfPCell(new iTextSharp.text.Paragraph(slide.Text, font));
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);   
        }
    }
}

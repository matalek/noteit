using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
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
    class HorizontalPrintStrategy : IPrintStrategy
    {
        public void Print(Note note, FileStream fs)
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter wri = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            // printing title
            Font titleFont = FontFactory.GetFont("Arial", 30, Font.BOLD);
            var title = new iTextSharp.text.Paragraph(note.Title, titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20;
            doc.Add(title);

            // NOTE: A4 points size: 595x842

            // 3 slides per each row
            int slidesPerRow = 3;
            
            PdfPTable table = new PdfPTable(slidesPerRow);
            int[] widths = { 270, 270, 270 };
            table.SetWidths(widths);

            int slidesPrinted = 0;

            while (slidesPrinted < note.SlidesList.Count)
            {
                for (int i = slidesPrinted; i < Math.Min(note.SlidesList.Count, slidesPrinted + slidesPerRow); i++)
                    PrintSlide(note.SlidesList[i], table);

                slidesPrinted += slidesPerRow;
            }

            doc.Add(table);
            doc.Close();
        }

        private void PrintSlide(Slide slide, PdfPTable table)
        {
            PdfPCell cell = new PdfPCell();

            if (slide.Image != null)
            {
                var pdfImage = iTextSharp.text.Image.GetInstance(BitmapHelper.ResizeBitmapToWidth(slide.Image, 250), System.Drawing.Imaging.ImageFormat.Bmp);
                cell.AddElement(pdfImage);
            }
            // else: PDF image is not set for this slide (PDF slides weren't imported or user decided to add additional slides)

            cell.BorderWidth = 0;

            var paragraph = new iTextSharp.text.Paragraph(slide.Text);
            paragraph.SpacingAfter = 30;
            paragraph.Alignment = Element.ALIGN_MIDDLE;
            cell.AddElement(paragraph);

            table.AddCell(cell);
        }
    }
}

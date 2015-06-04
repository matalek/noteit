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
    class HorizontalPrintStrategy : PrintStrategy
    {
        protected override PrintStrategy.OrientationType Orientation { get { return OrientationType.Horizontal; } }

        public override void Print(Note note, FileStream fs)
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), Margins[0], Margins[1], Margins[2], Margins[3]);
            PdfWriter wri = PdfWriter.GetInstance(doc, fs);

            TableHeader tevent = new TableHeader();
            wri.PageEvent = tevent;
            tevent.Header = note.Title;
            tevent.Orientation = OrientationType.Horizontal;

            doc.Open();

            PrintTitle(note, doc);

            // NOTE: A4 points size: 595x842

            // 3 slides per each row
            int slidesPerRow = 3;
            
            PdfPTable table = new PdfPTable(slidesPerRow);
            table.SetWidths(new int[] { 1, 1, 1 });

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

            Font font = FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 10);
            var paragraph = new iTextSharp.text.Paragraph(slide.Text, font);
            paragraph.SpacingAfter = 30;
            paragraph.Alignment = Element.ALIGN_MIDDLE;
            cell.AddElement(paragraph);

            table.AddCell(cell);
        }
    }
}

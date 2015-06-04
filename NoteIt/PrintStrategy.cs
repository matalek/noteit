using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NoteIt
{
    abstract class PrintStrategy
    {
        public abstract void Print(Note note, FileStream fs, bool withSlideNumbers);

        protected void PrintTitle(Note note, Document doc)
        {
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1250, 30, Font.BOLD);
            var title = new iTextSharp.text.Paragraph(note.Title, titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20;
            doc.Add(title);
        }

        protected enum OrientationType {Vertical, Horizontal};

        protected abstract OrientationType Orientation { get; }

        protected virtual int[] Margins { get { return new int[] { 10, 10, 70, 10 }; } }

        // creating header with title and page, from iTextSharp examples
        // source: http://sourceforge.net/p/itextsharp/code/HEAD/tree/book/iTextExamplesWeb/iTextExamplesWeb/iTextInAction2Ed/Chapter05/MovieCountries1.cs#l102
        protected class TableHeader : PdfPageEventHelper
        {
            /** The template with the total number of pages. */
            PdfTemplate total;

            /** The header text. */
            public string Header { get; set; }

            public OrientationType Orientation { get; set; }

            /**
             * Creates the PdfTemplate that will hold the total number of pages.
             * @see com.itextpdf.text.pdf.PdfPageEventHelper#onOpenDocument(
             *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
             */
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                total = writer.DirectContent.CreateTemplate(30, 16);
            }

            /**
             * Adds a header to every page
             * @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
             *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
             */
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                Debug.WriteLine("Koniec strony");
                PdfPTable table = new PdfPTable(3);
                try
                {
                    table.SetWidths(new int[] { 24, 24, 2 });
                    if (Orientation == OrientationType.Vertical)
                        table.TotalWidth = 527;
                    else
                        table.TotalWidth = 774;
                    table.LockedWidth = true;
                    table.DefaultCell.FixedHeight = 20;
                    table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
                    table.AddCell(Header);
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(string.Format("Page {0} of", writer.PageNumber));
                    PdfPCell cell = new PdfPCell(iTextSharp.text.Image.GetInstance(total));
                    cell.Border = Rectangle.BOTTOM_BORDER;
                    table.AddCell(cell);
                    if (Orientation == OrientationType.Vertical)
                        table.WriteSelectedRows(0, -1, 34, 803, writer.DirectContent);
                    else
                        table.WriteSelectedRows(0, -1, 34, 556, writer.DirectContent);
                }
                catch (DocumentException de)
                {
                    throw de;
                }
            }

            /**
             * Fills out the total number of pages before the document is closed.
             * @see com.itextpdf.text.pdf.PdfPageEventHelper#onCloseDocument(
             *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
             */
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                ColumnText.ShowTextAligned(
                  total, Element.ALIGN_LEFT,
                    /*
                     * NewPage() already called when closing the document; subtract 1
                    */
                  new Phrase((writer.PageNumber - 1).ToString()),
                  2, 2, 0
                );
            }
        }
    }
}

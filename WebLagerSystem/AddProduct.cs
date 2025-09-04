namespace WebLagerSystem
{
    public class AddProduct
    {
        public static string GetHtml()
        {
            return @"<!DOCTYPE html>
                <html>
                <head>
                    <title>Add Product</title>
                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
                </head>
                <body>
                        <div class=""modal"" id=""modal-js-example"">
                          <div class=""modal-background""></div>
                          <div class=""modal-card"">
                            <header class=""modal-card-head"">
                              <p class=""modal-card-title"">Add Product</p>
                              <button class=""delete"" aria-label=""close""></button>
                            </header>
                            <section class=""modal-card-body is-flex is-flex-direction-column"">
                                        <label for=""product-quantity"">Produkt ID</label>
                                        <input class=""input"" type=""text"" id=""product-id""/>
                                        <label for=""product-name"">Navn</label>
                                        <input class=""input"" type=""text"" id=""product-name""/>
                                        <label for=""product-quantity"">Antal</label>
                                        <input class=""input"" type=""number"" id=""product-quantity""/>
                            </section>
                            <footer class=""modal-card-foot"">
                              <div class=""buttons"">
                                <button class=""button is-success"">Save changes</button>
                                <button class=""button"">Cancel</button>
                              </div>
                            </footer>
                          </div>
                        </div>
                    <script>
                        document.addEventListener('DOMContentLoaded', () => {
                          // Functions to open and close a modal
                          function openModal($el) {
                            $el.classList.add('is-active');
                          }

                          function closeModal($el) {
                            $el.classList.remove('is-active');
                          }

                          function closeAllModals() {
                            (document.querySelectorAll('.modal') || []).forEach(($modal) => {
                              closeModal($modal);
                            });
                          }

                          // Add a click event on buttons to open a specific modal
                          (document.querySelectorAll('.js-modal-trigger') || []).forEach(($trigger) => {
                            const modal = $trigger.dataset.target;
                            const $target = document.getElementById(modal);

                            $trigger.addEventListener('click', () => {
                              openModal($target);
                            });
                          });

                          // Add a click event on various child elements to close the parent modal
                          (document.querySelectorAll('.modal-background, .modal-close, .modal-card-head .delete, .modal-card-foot .button') || []).forEach(($close) => {
                            const $target = $close.closest('.modal');

                            $close.addEventListener('click', () => {
                              closeModal($target);
                            });
                          });

                          // Add a keyboard event to close all modals
                          document.addEventListener('keydown', (event) => {
                            if(event.key === ""Escape"") {
                              closeAllModals();
                            }
                          });
                        });
                    </script>
                </body>
                </html>";
        }
    }
}

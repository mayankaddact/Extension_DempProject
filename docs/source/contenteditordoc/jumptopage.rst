============================
In place "Thank you" message
============================

The ShowFormPage custom submit action makes it possible to show a thank you message after submitting the form 
without the need of creating a dedicated seperate thank you page.

With this action, the form is replaced inline via ajax, there is no other page loaded.

How
===

Let’s create a simple form to let users subscribe to a kind of newsletter.

.. image:: jumptopage/show-form-page-1.png

The form contains a single email field and a submit button, with the submit action save data.

 

Now, let’s add a second page in the form (just like you would do for a multipage form).

.. image:: jumptopage/show-form-page-3.png

Give the page a name.

.. image:: jumptopage/show-form-page-4.png

Add some components on the thankyoupage.

.. image:: jumptopage/show-form-page-5.png

On the submit button: add the submit action “show form page” and select the thank you page.

.. image:: jumptopage/show-form-page-6.png


Result
======

.. image:: jumptopage/show-form-page-2.png

After submit:

.. image:: jumptopage/show-form-page-7.png